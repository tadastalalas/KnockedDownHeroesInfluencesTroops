using System;
using System.Collections.Generic;
using KnockedDownHeroesInfluencesTroops.Core;
using KnockedDownHeroesInfluencesTroops.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops.Missions;

// KDHIT-FEATURE: a hero knocked down in a field or siege battle changes troop morale, makes the attacker's troops cheer, shows a notification and plays a voice line | gate: setting:EnableThisModification
public class KnockedDownHeroesInfluencesTroopsMissionBehavior : MissionBehavior
{
    public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

    private float _elapsedTime;
    private bool _listsInitialized;

    private readonly List<Agent> _friendlyInfantryCaptains = [];
    private readonly List<Agent> _friendlyArchersCaptains = [];
    private readonly List<Agent> _friendlyCavalryCaptains = [];
    private readonly List<Agent> _friendlyHorseArchersCaptains = [];

    private readonly List<Agent> _enemyInfantryCaptains = [];
    private readonly List<Agent> _enemyArchersCaptains = [];
    private readonly List<Agent> _enemyCavalryCaptains = [];
    private readonly List<Agent> _enemyHorseArchersCaptains = [];

    private readonly Dictionary<Agent, List<Agent>> _troopsOfFormationCaptains = new();

    // Troop lists are reused between rescans, so a rescan allocates no new lists.
    private readonly Stack<List<Agent>> _spareTroopLists = new();

    private static bool IsSupportedBattle => Mission.Current is { } mission && (mission.IsFieldBattle || mission.IsSiegeBattle);

    public override void OnMissionTick(float dt)
    {
        base.OnMissionTick(dt);

        if (!ModSettings.EnableThisModification)
            return;

        MainSetup(dt);
        MissionUtilities.ProcessCheerQueue(dt);
    }

    private void MainSetup(float dt)
    {
        if (!IsSupportedBattle)
            return;

        _elapsedTime += dt;
        if (_listsInitialized && _elapsedTime < Math.Max(1, ModSettings.UpdateIntervalInSeconds))
            return;

        InitializeTeamsFormationsCaptainsAndTroops();
        _listsInitialized = true;
        _elapsedTime = 0f;
    }

    private void InitializeTeamsFormationsCaptainsAndTroops()
    {
        ClearAllLists();

        int totalTeamsCount = 0;
        int friendlyTeamsCount = 0;
        int enemyTeamsCount = 0;

        foreach (Team team in Mission.Current.Teams)
        {
            totalTeamsCount++;
            if (team.IsPlayerAlly)
            {
                friendlyTeamsCount++;
                ProcessTeamFormations(team, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains);
            }
            else
            {
                enemyTeamsCount++;
                ProcessTeamFormations(team, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains);
            }
        }

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"Teams: total {totalTeamsCount}, friendly {friendlyTeamsCount}, enemy {enemyTeamsCount}.");
    }

    private void ClearAllLists()
    {
        _friendlyInfantryCaptains.Clear();
        _friendlyArchersCaptains.Clear();
        _friendlyCavalryCaptains.Clear();
        _friendlyHorseArchersCaptains.Clear();
        _enemyInfantryCaptains.Clear();
        _enemyArchersCaptains.Clear();
        _enemyCavalryCaptains.Clear();
        _enemyHorseArchersCaptains.Clear();

        foreach (List<Agent> troops in _troopsOfFormationCaptains.Values)
        {
            troops.Clear();
            _spareTroopLists.Push(troops);
        }

        _troopsOfFormationCaptains.Clear();
    }

    private void ProcessTeamFormations(Team team, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains)
    {
        int infantryFormationsCount = 0;
        int archersFormationsCount = 0;
        int cavalryFormationsCount = 0;
        int horseArchersFormationsCount = 0;

        foreach (Formation formation in team.FormationsIncludingEmpty)
        {
            if (formation.CountOfUnits <= 0)
                continue;

            FormationKind formationKind = MissionUtilities.GetFormationKind(formation);
            switch (formationKind)
            {
                case FormationKind.Infantry:
                    infantryFormationsCount++;
                    break;
                case FormationKind.Archers:
                    archersFormationsCount++;
                    break;
                case FormationKind.Cavalry:
                    cavalryFormationsCount++;
                    break;
                case FormationKind.HorseArchers:
                    horseArchersFormationsCount++;
                    break;
            }

            ProcessFormation(formation, formationKind, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains);
        }

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"{MissionUtilities.SideLogLabel(team.IsPlayerAlly)} team formations: infantry {infantryFormationsCount}, archers {archersFormationsCount}, cavalry {cavalryFormationsCount}, horse archers {horseArchersFormationsCount}.");
    }

    private void ProcessFormation(Formation formation, FormationKind formationKind, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains)
    {
        Agent? captain = formation.Captain;
        if (captain == null)
            return;

        AddCaptainToFormationLists(captain, formationKind, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains);

        // A captain of several formations keeps one list, refilled like before.
        if (_troopsOfFormationCaptains.TryGetValue(captain, out List<Agent>? troops))
        {
            troops.Clear();
        }
        else
        {
            troops = RentTroopList();
            _troopsOfFormationCaptains[captain] = troops;
        }

        foreach (Agent agent in formation.Team.ActiveAgents)
            if (agent.Formation != null && agent.Formation.Captain == captain)
                troops.Add(agent);

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"{MissionUtilities.SideLogLabel(formation.Team.IsPlayerAlly)} {formationKind} formation captain: {captain.Name}, troops in formation: {troops.Count}.");
    }

    private List<Agent> RentTroopList() => _spareTroopLists.Count > 0 ? _spareTroopLists.Pop() : [];

    private static void AddCaptainToFormationLists(Agent captain, FormationKind formationKind, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains)
    {
        switch (formationKind)
        {
            case FormationKind.Infantry:
                infantryCaptains.Add(captain);
                break;
            case FormationKind.Archers:
                archersCaptains.Add(captain);
                break;
            case FormationKind.Cavalry:
                cavalryCaptains.Add(captain);
                break;
            case FormationKind.HorseArchers:
                horseArchersCaptains.Add(captain);
                break;
        }
    }

    private bool IsCaptain(Agent agent) => MissionUtilities.IsAgentCaptain(agent, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains);

    public override void OnAgentRemoved(Agent? affectedAgent, Agent? affectorAgent, AgentState agentState, KillingBlow blow)
    {
        base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

        if (!ModSettings.EnableThisModification || !IsSupportedBattle ||
            affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent ||
            affectedAgent.Team == null || affectorAgent.Team == null ||
            !affectedAgent.Team.IsEnemyOf(affectorAgent.Team)) return;

        if (!affectedAgent.IsHero)
            return;

        PlayFallReactionVoiceLineIfApplicable(affectedAgent);

        if (affectorAgent.IsHero)
            HeroKnockedDownAgent(affectorAgent, affectedAgent);
        else
            SimpleTroopKnockedDownAgent(affectorAgent, affectedAgent);

        if (ModSettings.LoggingEnabled)
            MissionUtilities.LogKnockdown(affectorAgent, affectedAgent);
    }

    private void PlayFallReactionVoiceLineIfApplicable(Agent affectedAgent)
    {
        if (MissionUtilities.IsAgentGeneral(affectedAgent))
            MissionUtilities.PlayHeroFallReactionVoiceLine(affectedAgent, victimIsGeneral: true);
        else if (IsCaptain(affectedAgent))
            MissionUtilities.PlayHeroFallReactionVoiceLine(affectedAgent, victimIsGeneral: false);
    }

    private void HeroKnockedDownAgent(Agent attackerAgent, Agent victimAgent)
    {
        if (MissionUtilities.IsAgentGeneral(attackerAgent))
            GeneralKnockedDownAgent(attackerAgent, victimAgent);
        else if (IsCaptain(attackerAgent))
            CaptainKnockedDownAgent(attackerAgent, victimAgent);
        else
            UnassignedHeroKnockedDownAgent(attackerAgent, victimAgent);
    }

    private void GeneralKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
    {
        MissionUtilities.SetWantsToYellForTeam(affectorAgent.Team);

        if (MissionUtilities.IsAgentGeneral(affectedAgent))
        {
            MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -ModSettings.MoraleChangeWhenGeneralHeroKillsGeneralHero);
            MissionUtilities.UpdateTeamMorale(affectorAgent.Team, ModSettings.MoraleGainWhenGeneralHeroKillsGeneralHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
        }
        else if (IsCaptain(affectedAgent))
        {
            MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, affectedAgent, -ModSettings.MoraleChangeWhenGeneralHeroKillsCaptainHero);
            MissionUtilities.UpdateTeamMorale(affectorAgent.Team, ModSettings.MoraleGainWhenGeneralHeroKillsCaptainHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
        }
        else
        {
            MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, Balance.GeneralFallRange, -ModSettings.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, Balance.GeneralFallRange, ModSettings.MoraleGainWhenGeneralHeroKillsUnassignedHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
        }
    }

    private void CaptainKnockedDownAgent(Agent attackerAgent, Agent victimAgent)
    {
        MissionUtilities.SetWantsToYellForFormation(_troopsOfFormationCaptains[attackerAgent]);

        if (MissionUtilities.IsAgentGeneral(victimAgent))
        {
            MissionUtilities.UpdateTeamMorale(victimAgent.Team, -ModSettings.MoraleChangeWhenCaptainHeroKillsGeneralHero);
            MissionUtilities.UpdateTeamMorale(attackerAgent.Team, ModSettings.MoraleGainWhenCaptainHeroKillsGeneralHero);
            ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
        }
        else if (IsCaptain(victimAgent))
        {
            MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, victimAgent, -ModSettings.MoraleChangeWhenCaptainHeroKillsCaptainHero);
            MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, attackerAgent, ModSettings.MoraleGainWhenCaptainHeroKillsCaptainHero);
            ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
        }
        else
        {
            MissionUtilities.UpdateMoraleForNearbyAgents(victimAgent.Team, victimAgent, Balance.UnassignedHeroFallRange + Balance.CaptainAttackerExtraRange, -ModSettings.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
            MissionUtilities.UpdateMoraleForNearbyAgents(attackerAgent.Team, attackerAgent, Balance.UnassignedHeroFallRange + Balance.CaptainAttackerExtraRange, ModSettings.MoraleGainWhenCaptainHeroKillsUnassignedHero);
            ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
        }
    }

    private void UnassignedHeroKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
    {
        if (MissionUtilities.IsAgentGeneral(affectedAgent))
        {
            MissionUtilities.SetWantsToYellInRange(affectorAgent, Balance.GeneralFallRange + Balance.UnassignedHeroAttackerExtraRange);
            MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -ModSettings.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
            MissionUtilities.UpdateTeamMorale(affectorAgent.Team, ModSettings.MoraleGainWhenUnassignedHeroKillsGeneralHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
        }
        else if (IsCaptain(affectedAgent))
        {
            MissionUtilities.SetWantsToYellInRange(affectorAgent, Balance.CaptainFallRange + Balance.UnassignedHeroAttackerExtraRange);
            MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, affectedAgent, -ModSettings.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, Balance.CaptainFallRange + Balance.UnassignedHeroAttackerExtraRange, ModSettings.MoraleGainWhenUnassignedHeroKillsCaptainHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
        }
        else
        {
            MissionUtilities.SetWantsToYellInRange(affectorAgent, Balance.UnassignedHeroFallRange + Balance.UnassignedHeroAttackerExtraRange);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, Balance.UnassignedHeroFallRange, -ModSettings.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, Balance.UnassignedHeroFallRange, ModSettings.MoraleGainWhenUnassignedHeroKillsUnassignedHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
        }
    }

    private void SimpleTroopKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
    {
        if (MissionUtilities.IsAgentGeneral(affectedAgent))
        {
            MissionUtilities.SetWantsToYellInRange(affectorAgent, Balance.GeneralFallRange);
            MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -ModSettings.MoraleChangeWhenTroopKillsGeneralHero);
            MissionUtilities.UpdateTeamMorale(affectorAgent.Team, ModSettings.MoraleGainWhenTroopKillsGeneralHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
        }
        else if (IsCaptain(affectedAgent))
        {
            MissionUtilities.SetWantsToYellInRange(affectorAgent, Balance.CaptainFallRange);
            MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, affectedAgent, -ModSettings.MoraleChangeWhenTroopKillsCaptainHero);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, Balance.CaptainFallRange, ModSettings.MoraleGainWhenTroopKillsCaptainHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
        }
        else
        {
            MissionUtilities.SetWantsToYellInRange(affectorAgent, Balance.UnassignedHeroFallRange);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, Balance.UnassignedHeroFallRange, -ModSettings.MoraleChangeWhenTroopKillsUnassignedHero);
            MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, Balance.UnassignedHeroFallRange, ModSettings.MoraleGainWhenTroopKillsUnassignedHero);
            ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
        }
    }

    private static void ShowOnScreenNotification(Agent affectorAgent, Agent affectedAgent, Action<Agent, Agent> displayNotification)
    {
        if (ModSettings.ShowOnScreenNotifications)
            displayNotification(affectorAgent, affectedAgent);
    }

    public override void OnRemoveBehavior()
    {
        base.OnRemoveBehavior();
        MissionUtilities.ResetCheerState();
    }
}
