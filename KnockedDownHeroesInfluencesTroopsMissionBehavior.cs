using System;
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

        private readonly MCMSettings _settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        private const int RangeForTroopsToReactToUnassignedHeroFall = 10;
        private const int RangeForTroopsToReactToCaptainHeroFall = 20;
        private const int RangeForTroopsToReactToGeneralHeroFall = 30;

        private float _elapsedTime;
        private bool _listsInitialized;

        private readonly List<Agent> _friendlyInfantryCaptains = new();
        private readonly List<Agent> _friendlyArchersCaptains = new();
        private readonly List<Agent> _friendlyCavalryCaptains = new();
        private readonly List<Agent> _friendlyHorseArchersCaptains = new();

        private readonly List<Agent> _enemyInfantryCaptains = new();
        private readonly List<Agent> _enemyArchersCaptains = new();
        private readonly List<Agent> _enemyCavalryCaptains = new();
        private readonly List<Agent> _enemyHorseArchersCaptains = new();

        private readonly Dictionary<Agent, List<Agent>> _troopsOfFormationCaptains = new();

        private readonly List<(string message, Color color)> _storedLogMessagesList = new();

        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);

            if (!_settings.EnableThisModification)
                return;

            MainSetup(dt);
            MissionUtilities.ProcessCheerQueue(dt);
        }

        private void MainSetup(float dt)
        {
            if (Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle))
                return;

            _elapsedTime += dt;
            if (_listsInitialized && _elapsedTime < Math.Max(1, _settings.UpdateIntervalInSeconds))
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
                    ProcessTeamFormations(team, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, Colors.Yellow);
                }
                else
                {
                    enemyTeamsCount++;
                    ProcessTeamFormations(team, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains, Colors.Red);
                }
            }

            if (_settings.LoggingEnabled)
            {
                _storedLogMessagesList.Add(($"Enemy teams: {enemyTeamsCount}", Colors.Red));
                _storedLogMessagesList.Add(($"Friendly teams: {friendlyTeamsCount}", Colors.Yellow));
                _storedLogMessagesList.Add(($"Total teams: {totalTeamsCount}", Colors.White));
                _storedLogMessagesList.Reverse();
                MissionUtilities.ShowLogs(_storedLogMessagesList);
            }
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
            _troopsOfFormationCaptains.Clear();
            _storedLogMessagesList.Clear();
        }

        private void ProcessTeamFormations(Team team, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains, Color logColor)
        {
            int infantryFormationsCount = 0;
            int archersFormationsCount = 0;
            int cavalryFormationsCount = 0;
            int horseArchersFormationsCount = 0;

            foreach (Formation formation in team.FormationsIncludingEmpty.Where(f => f.CountOfUnits > 0))
            {
                if (formation.QuerySystem.IsInfantryFormation)
                    infantryFormationsCount++;
                else if (formation.QuerySystem.IsRangedFormation)
                    archersFormationsCount++;
                else if (formation.QuerySystem.IsCavalryFormation)
                    cavalryFormationsCount++;
                else if (formation.QuerySystem.IsRangedCavalryFormation)
                    horseArchersFormationsCount++;

                ProcessFormation(formation, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains, logColor);
            }

            if (_settings.LoggingEnabled)
            {
                _storedLogMessagesList.Add(($"Horse Archers formations: {horseArchersFormationsCount}", logColor));
                _storedLogMessagesList.Add(($"Cavalry formations: {cavalryFormationsCount}", logColor));
                _storedLogMessagesList.Add(($"Archers formations: {archersFormationsCount}", logColor));
                _storedLogMessagesList.Add(($"Infantry formations: {infantryFormationsCount}", logColor));
            }
        }

        private void ProcessFormation(Formation formation, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains, Color logColor)
        {
            if (formation?.Captain == null)
                return;

            AddCaptainToFormationLists(formation, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains);

            var troops = new List<Agent> { };
            _troopsOfFormationCaptains[formation.Captain] = troops;

            foreach (var agent in formation.Team.ActiveAgents)
                if (agent.Formation != null && agent.Formation.Captain == formation.Captain)
                    troops.Add(agent);

            if (!_settings.LoggingEnabled)
                return;
            
            _storedLogMessagesList.Add(($"{GetFormationType(formation)} formation captain: {formation.Captain.Name}", logColor));
            _storedLogMessagesList.Add(($"Troops in formation: {troops.Count}", logColor));
        }

        private static string GetFormationType(Formation formation)
        {
            if (formation.QuerySystem.IsInfantryFormation)
                return "Infantry";
            if (formation.QuerySystem.IsRangedFormation)
                return "Archers";
            if (formation.QuerySystem.IsCavalryFormation)
                return "Cavalry";
            if (formation.QuerySystem.IsRangedCavalryFormation)
                return "Horse Archers";
            return "Unknown";
        }


        private void AddCaptainToFormationLists(Formation formation, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains)
        {
            if (formation.QuerySystem.IsInfantryFormation)
                infantryCaptains.Add(formation.Captain);
            else if (formation.QuerySystem.IsRangedFormation)
                archersCaptains.Add(formation.Captain);
            else if (formation.QuerySystem.IsCavalryFormation)
                cavalryCaptains.Add(formation.Captain);
            else if (formation.QuerySystem.IsRangedCavalryFormation)
                horseArchersCaptains.Add(formation.Captain);
        }

        public override void OnAgentRemoved(Agent? affectedAgent, Agent? affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!_settings.EnableThisModification || Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle) ||
                affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent ||
                affectedAgent.Team == null || affectorAgent.Team == null) return;

            if (!affectedAgent.IsHero)
                return;

            if (affectorAgent.IsHero)
                HeroKnockedDownAgent(affectorAgent, affectedAgent);
            else
                SimpleTroopKnockedDownAgent(affectorAgent, affectedAgent);

            if (_settings.LoggingEnabled)
                MissionUtilities.DisplayKnockdownMessage(affectorAgent, affectedAgent);
        }

        private void HeroKnockedDownAgent(Agent attackerAgent, Agent victimAgent)
        {
            if (MissionUtilities.IsAgentGeneral(attackerAgent))
                GeneralKnockedDownAgent(attackerAgent, victimAgent);
            else if (MissionUtilities.IsAgentCaptain(attackerAgent, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains))
                CaptainKnockedDownAgent(attackerAgent, victimAgent);
            else
                UnassignedHeroKnockedDownAgent(attackerAgent, victimAgent);
        }

        private void GeneralKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
        {
            MissionUtilities.SetWantsToYellForTeam(affectorAgent.Team);

            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -_settings.MoraleChangeWhenGeneralHeroKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(affectorAgent.Team, _settings.MoraleGainWhenGeneralHeroKillsGeneralHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains))
            {
                MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, affectedAgent, -_settings.MoraleChangeWhenGeneralHeroKillsCaptainHero);
                MissionUtilities.UpdateTeamMorale(affectorAgent.Team, _settings.MoraleGainWhenGeneralHeroKillsCaptainHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, RangeForTroopsToReactToGeneralHeroFall, -_settings.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, RangeForTroopsToReactToGeneralHeroFall, _settings.MoraleGainWhenGeneralHeroKillsUnassignedHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void CaptainKnockedDownAgent(Agent attackerAgent, Agent victimAgent)
        {
            MissionUtilities.SetWantsToYellForFormation(_troopsOfFormationCaptains[attackerAgent]);

            if (MissionUtilities.IsAgentGeneral(victimAgent))
            {
                MissionUtilities.UpdateTeamMorale(victimAgent.Team, -_settings.MoraleChangeWhenCaptainHeroKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(attackerAgent.Team, _settings.MoraleGainWhenCaptainHeroKillsGeneralHero);
                ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(victimAgent, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains))
            {
                MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, victimAgent, -_settings.MoraleChangeWhenCaptainHeroKillsCaptainHero);
                MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, attackerAgent, _settings.MoraleGainWhenCaptainHeroKillsCaptainHero);
                ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.UpdateMoraleForNearbyAgents(victimAgent.Team, victimAgent, RangeForTroopsToReactToUnassignedHeroFall + 10, -_settings.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(attackerAgent.Team, attackerAgent, RangeForTroopsToReactToUnassignedHeroFall + 10, _settings.MoraleGainWhenCaptainHeroKillsUnassignedHero);
                ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void UnassignedHeroKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
        {
            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, RangeForTroopsToReactToGeneralHeroFall + 5);
                MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -_settings.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(affectorAgent.Team, _settings.MoraleGainWhenUnassignedHeroKillsGeneralHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, RangeForTroopsToReactToCaptainHeroFall + 5);
                MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, affectedAgent, -_settings.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, RangeForTroopsToReactToCaptainHeroFall + 5, _settings.MoraleGainWhenUnassignedHeroKillsCaptainHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, RangeForTroopsToReactToUnassignedHeroFall + 5);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, RangeForTroopsToReactToUnassignedHeroFall, -_settings.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, RangeForTroopsToReactToUnassignedHeroFall, _settings.MoraleGainWhenUnassignedHeroKillsUnassignedHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void SimpleTroopKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
        {
            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, RangeForTroopsToReactToGeneralHeroFall);
                MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -_settings.MoraleChangeWhenTroopKillsGeneralHero);
                MissionUtilities.UpdateTeamMorale(affectorAgent.Team, _settings.MoraleGainWhenTroopKillsGeneralHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, _friendlyInfantryCaptains, _friendlyArchersCaptains, _friendlyCavalryCaptains, _friendlyHorseArchersCaptains, _enemyInfantryCaptains, _enemyArchersCaptains, _enemyCavalryCaptains, _enemyHorseArchersCaptains))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, RangeForTroopsToReactToCaptainHeroFall);
                MissionUtilities.UpdateFormationMorale(_troopsOfFormationCaptains, affectedAgent, -_settings.MoraleChangeWhenTroopKillsCaptainHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, RangeForTroopsToReactToCaptainHeroFall, _settings.MoraleGainWhenTroopKillsCaptainHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, RangeForTroopsToReactToUnassignedHeroFall);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, RangeForTroopsToReactToUnassignedHeroFall, -_settings.MoraleChangeWhenTroopKillsUnassignedHero);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectorAgent.Team, affectorAgent, RangeForTroopsToReactToUnassignedHeroFall, _settings.MoraleGainWhenTroopKillsUnassignedHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void ShowOnScreenNotification(Agent affectorAgent, Agent affectedAgent, Action<Agent, Agent> displayNotification)
        {
            if (_settings.ShowOnScreenNotifications)
                displayNotification(affectorAgent, affectedAgent);
        }
        
        public override void OnRemoveBehavior()
        {
            base.OnRemoveBehavior();
            MissionUtilities.ResetCheerState();
        }
    }
}