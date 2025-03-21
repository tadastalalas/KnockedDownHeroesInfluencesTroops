using System.Collections.Generic;
using System.Linq;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public class KnockedDownHeroesInfluencesTroopsMissionBehavior : MissionBehavior
    {
        public override MissionBehaviorType BehaviorType => MissionBehaviorType.Other;

        private readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        private const int rangeForTroopsToRespondToUnassignedHero = 10;
        private const int rangeForTroopsToRespondToCaptainHero = 20;
        private const int rangeForTroopsToRespondToGeneralHero = 30;

        private float elapsedTime = 0f;

        private readonly List<Formation> enemyTeamsFormations = new();
        private readonly List<Formation> friendlyTeamsFormations = new();

        private readonly List<Formation> friendlyTeamsInfantryFormations = new();
        private readonly List<Formation> friendlyTeamsArchersFormations = new();
        private readonly List<Formation> friendlyTeamsCavalryFormations = new();
        private readonly List<Formation> friendlyTeamsHorseArchersFormations = new();
        private readonly List<Formation> enemyTeamsInfantryFormations = new();
        private readonly List<Formation> enemyTeamsArchersFormations = new();
        private readonly List<Formation> enemyTeamsCavalryFormations = new();
        private readonly List<Formation> enemyTeamsHorseArchersFormations = new();

        private readonly List<Agent> friendlyInfantryCaptains = new();
        private readonly List<Agent> friendlyArchersCaptains = new();
        private readonly List<Agent> friendlyCavalryCaptains = new();
        private readonly List<Agent> friendlyHorseArchersCaptains = new();
        private readonly List<Agent> enemyInfantryCaptains = new();
        private readonly List<Agent> enemyArchersCaptains = new();
        private readonly List<Agent> enemyCavalryCaptains = new();
        private readonly List<Agent> enemyHorseArchersCaptains = new();

        private readonly Dictionary<Agent, List<Agent>> troopsOfFormationCaptains = new();

        private readonly List<(string message, Color color)> captainLogMessages = new();

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (!settings.EnableThisModification)
                return;

            MainSetup(dt);
        }

        private void MainSetup(float dt)
        {
            if (Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle))
                return;

            elapsedTime += dt;
            if (elapsedTime < settings.UpdateIntervalInSeconds)
                return;

            InitializeTeamsFormationsCaptainsAndTroops();

            elapsedTime = 0f;
        }

        private void InitializeTeamsFormationsCaptainsAndTroops()
        {
            MissionUtilities.ClearAllLists(
                enemyTeamsFormations, friendlyTeamsFormations,
                friendlyTeamsInfantryFormations, friendlyTeamsArchersFormations,
                friendlyTeamsCavalryFormations, friendlyTeamsHorseArchersFormations,
                enemyTeamsInfantryFormations, enemyTeamsArchersFormations,
                enemyTeamsCavalryFormations, enemyTeamsHorseArchersFormations,
                friendlyInfantryCaptains, friendlyArchersCaptains,
                friendlyCavalryCaptains, friendlyHorseArchersCaptains,
                enemyInfantryCaptains, enemyArchersCaptains,
                enemyCavalryCaptains, enemyHorseArchersCaptains);

            int totalTeamsCount = 0;
            int friendlyTeamsCount = 0;
            int enemyTeamsCount = 0;

            foreach (Team team in Mission.Current.Teams)
            {
                totalTeamsCount++;
                if (team.IsPlayerAlly)
                {
                    friendlyTeamsCount++;
                    ProcessTeamFormations(team, friendlyTeamsFormations, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, Colors.Yellow, captainLogMessages);
                }
                else
                {
                    enemyTeamsCount++;
                    ProcessTeamFormations(team, enemyTeamsFormations, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains, Colors.Red, captainLogMessages);
                }
            }

            if (settings.LoggingEnabled)
            {
                MissionUtilities.LogTeamAndFormationCounts(
                    totalTeamsCount, friendlyTeamsCount, enemyTeamsCount,
                    friendlyTeamsFormations, enemyTeamsFormations,
                    friendlyTeamsInfantryFormations, friendlyTeamsArchersFormations,
                    friendlyTeamsCavalryFormations, friendlyTeamsHorseArchersFormations,
                    enemyTeamsInfantryFormations, enemyTeamsArchersFormations,
                    enemyTeamsCavalryFormations, enemyTeamsHorseArchersFormations);
                MissionUtilities.LogCaptainMessages(captainLogMessages);
            }
        }

        private void ProcessTeamFormations(Team team, List<Formation> teamFormations, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains, Color logColor, List<(string message, Color color)> logMessages)
        {
            foreach (Formation formation in team.FormationsIncludingEmpty.Where(f => f.CountOfUnits > 0))
            {
                teamFormations.Add(formation);
                ProcessFormation(formation, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains, logColor, logMessages);
            }
        }

        private void ProcessFormation(Formation formation, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains, Color logColor, List<(string message, Color color)> logMessages)
        {
            if (formation == null || formation.Captain == null)
                return;

            AddCaptainToFormationLists(formation, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains);

            logMessages.Add(($"Formation {formation.Index} captain: {formation.Captain.Name}", logColor));
            troopsOfFormationCaptains[formation.Captain] = new List<Agent>();

            foreach (var agent in formation.Team.ActiveAgents)
                if (agent.Formation != null && agent.Formation.Captain == formation.Captain)
                    troopsOfFormationCaptains[formation.Captain].Add(agent);

            logMessages.Add(($"Troops in formation: {troopsOfFormationCaptains[formation.Captain].Count}", logColor));
        }

        private void AddCaptainToFormationLists(Formation formation, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains)
        {
            if (formation.QuerySystem.IsInfantryFormation)
            {
                infantryCaptains.Add(formation.Captain);
                if (formation.Team.IsPlayerAlly)
                    friendlyTeamsInfantryFormations.Add(formation);
                else
                    enemyTeamsInfantryFormations.Add(formation);
            }
            else if (formation.QuerySystem.IsRangedFormation)
            {
                archersCaptains.Add(formation.Captain);
                if (formation.Team.IsPlayerAlly)
                    friendlyTeamsArchersFormations.Add(formation);
                else
                    enemyTeamsArchersFormations.Add(formation);
            }
            else if (formation.QuerySystem.IsCavalryFormation)
            {
                cavalryCaptains.Add(formation.Captain);
                if (formation.Team.IsPlayerAlly)
                    friendlyTeamsCavalryFormations.Add(formation);
                else
                    enemyTeamsCavalryFormations.Add(formation);
            }
            else if (formation.QuerySystem.IsRangedCavalryFormation)
            {
                horseArchersCaptains.Add(formation.Captain);
                if (formation.Team.IsPlayerAlly)
                    friendlyTeamsHorseArchersFormations.Add(formation);
                else
                    enemyTeamsHorseArchersFormations.Add(formation);
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!settings.EnableThisModification || Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle) ||
                affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent || !affectedAgent.IsHero) return;

            if (settings.LoggingEnabled)
                MissionUtilities.DisplayKnockdownMessage(affectorAgent, affectedAgent);

            if (affectorAgent.IsHero)
            {
                if (MissionUtilities.IsAgentGeneral(affectorAgent))
                    HandleGeneralKnockdown(affectorAgent, affectedAgent);
                else if (MissionUtilities.IsAgentCaptain(affectorAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
                    HandleCaptainKnockdown(affectorAgent, affectedAgent);
                else
                    HandleUnassignedHeroKnockdown(affectorAgent, affectedAgent);
            }
            else
            {
                HandleTroopKnockdown(affectorAgent, affectedAgent);
            }
        }

        private void HandleGeneralKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            MissionUtilities.SetWantsToYellForTeam(affectorAgent.Team);

            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.ModifyTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenGeneralHeroKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.ModifyFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenGeneralHeroKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectedAgent.Team, affectorAgent, rangeForTroopsToRespondToGeneralHero, -settings.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void HandleCaptainKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            var captain = MissionUtilities.FindCaptainForAgent(affectorAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains);
            if (captain != null)
                MissionUtilities.SetWantsToYellForFormation(troopsOfFormationCaptains[captain]);

            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.ModifyTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenCaptainHeroKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.ModifyFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenCaptainHeroKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectedAgent.Team, affectorAgent, rangeForTroopsToRespondToCaptainHero, -settings.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void HandleUnassignedHeroKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToGeneralHero, 0, true);
                MissionUtilities.ModifyTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToCaptainHero, 0, true);
                MissionUtilities.ModifyFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToUnassignedHero, 0, true);
                MissionUtilities.ChangeMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, rangeForTroopsToRespondToUnassignedHero, -settings.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }

        private void HandleTroopKnockdown(Agent affectorAgent, Agent affectedAgent)
        {
            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToGeneralHero, 0, true);
                MissionUtilities.ModifyTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenTroopKillsGeneralHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls(affectorAgent, affectedAgent);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToCaptainHero, 0, true);
                MissionUtilities.ModifyFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenTroopKillsCaptainHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls(affectorAgent, affectedAgent);
            }
            else
            {
                MissionUtilities.ChangeMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, rangeForTroopsToRespondToUnassignedHero, 0, true);
                MissionUtilities.ChangeMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, rangeForTroopsToRespondToUnassignedHero, -settings.MoraleChangeWhenTroopKillsUnassignedHero);
                if (settings.ShowOnScreenNotifications)
                    MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls(affectorAgent, affectedAgent);
            }
        }
    }
}