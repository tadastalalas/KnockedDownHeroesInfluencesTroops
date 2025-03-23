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

        private readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        private const int rangeForTroopsToReactToUnassignedHeroFall = 10;
        private const int rangeForTroopsToReactToCaptainHeroFall = 20;
        private const int rangeForTroopsToReactToGeneralHeroFall = 30;

        private float elapsedTime = 0f;

        private readonly List<Formation> friendlyTeamsFormations = new();
        private readonly List<Formation> enemyTeamsFormations = new();

        private readonly List<Formation> friendlyTeamsInfantryFormations = new();
        private readonly List<Formation> friendlyTeamsArchersFormations = new();
        private readonly List<Formation> friendlyTeamsCavalryFormations = new();
        private readonly List<Formation> friendlyTeamsHorseArchersFormations = new();

        private readonly List<Formation> enemyTeamsInfantryFormations = new();
        private readonly List<Formation> enemyTeamsArchersFormations = new();
        private readonly List<Formation> enemyTeamsCavalryFormations = new();
        private readonly List<Formation> enemyTeamsHorseArchersFormations = new();

        private readonly List<Agent> friendlyHeroes = new();
        private readonly List<Agent> enemyHeroes = new();

        private readonly List<Agent> friendlyInfantryCaptains = new();
        private readonly List<Agent> friendlyArchersCaptains = new();
        private readonly List<Agent> friendlyCavalryCaptains = new();
        private readonly List<Agent> friendlyHorseArchersCaptains = new();

        private readonly List<Agent> enemyInfantryCaptains = new();
        private readonly List<Agent> enemyArchersCaptains = new();
        private readonly List<Agent> enemyCavalryCaptains = new();
        private readonly List<Agent> enemyHorseArchersCaptains = new();

        private readonly Dictionary<Agent, List<Agent>> troopsOfFormationCaptains = new();

        private readonly List<(string message, Color color)> storedLogMessagesList = new();

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
                    ProcessTeamFormations(team, friendlyTeamsFormations, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, Colors.Yellow);
                }
                else
                {
                    enemyTeamsCount++;
                    ProcessTeamFormations(team, enemyTeamsFormations, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains, Colors.Red);
                }
            }

            storedLogMessagesList.Add(($"Enemy teams: {enemyTeamsCount}", Colors.Red));
            storedLogMessagesList.Add(($"Friendly teams: {friendlyTeamsCount}", Colors.Yellow));
            storedLogMessagesList.Add(($"Total teams: {totalTeamsCount}", Colors.White));

            if (settings.LoggingEnabled)
            {
                storedLogMessagesList.Reverse();
                MissionUtilities.ShowLogs(storedLogMessagesList);
            }
        }

        private void ClearAllLists()
        {
            friendlyTeamsFormations.Clear();
            enemyTeamsFormations.Clear();
            friendlyTeamsInfantryFormations.Clear();
            friendlyTeamsArchersFormations.Clear();
            friendlyTeamsCavalryFormations.Clear();
            friendlyTeamsHorseArchersFormations.Clear();
            enemyTeamsInfantryFormations.Clear();
            enemyTeamsArchersFormations.Clear();
            enemyTeamsCavalryFormations.Clear();
            enemyTeamsHorseArchersFormations.Clear();
            friendlyHeroes.Clear();
            enemyHeroes.Clear();
            friendlyInfantryCaptains.Clear();
            friendlyArchersCaptains.Clear();
            friendlyCavalryCaptains.Clear();
            friendlyHorseArchersCaptains.Clear();
            enemyInfantryCaptains.Clear();
            enemyArchersCaptains.Clear();
            enemyCavalryCaptains.Clear();
            enemyHorseArchersCaptains.Clear();
        }

        private void ProcessTeamFormations(Team team, List<Formation> teamFormations, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains, Color logColor)
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

                teamFormations.Add(formation);
                ProcessFormation(formation, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains, logColor);
            }

            storedLogMessagesList.Add(($"Horse Archers formations: {horseArchersFormationsCount}", logColor));
            storedLogMessagesList.Add(($"Cavalry formations: {cavalryFormationsCount}", logColor));
            storedLogMessagesList.Add(($"Archers formations: {archersFormationsCount}", logColor));
            storedLogMessagesList.Add(($"Infantry formations: {infantryFormationsCount}", logColor));
        }

        private void ProcessFormation(Formation formation, List<Agent> infantryCaptains, List<Agent> archersCaptains, List<Agent> cavalryCaptains, List<Agent> horseArchersCaptains, Color logColor)
        {
            if (formation == null || formation.Captain == null)
                return;

            AddCaptainToFormationLists(formation, infantryCaptains, archersCaptains, cavalryCaptains, horseArchersCaptains);

            troopsOfFormationCaptains[formation.Captain] = new List<Agent>();

            foreach (var agent in formation.Team.ActiveAgents)
                if (agent.Formation != null && agent.Formation.Captain == formation.Captain)
                    troopsOfFormationCaptains[formation.Captain].Add(agent);

            string formationType = GetFormationType(formation);
            storedLogMessagesList.Add(($"{formationType} formation captain: {formation.Captain.Name}", logColor));
            storedLogMessagesList.Add(($"Troops in formation: {troopsOfFormationCaptains[formation.Captain].Count}", logColor));
        }

        private string GetFormationType(Formation formation)
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
            {
                infantryCaptains.Add(formation.Captain);
                AddFormationToTeamList(formation, friendlyTeamsInfantryFormations, enemyTeamsInfantryFormations);
            }
            else if (formation.QuerySystem.IsRangedFormation)
            {
                archersCaptains.Add(formation.Captain);
                AddFormationToTeamList(formation, friendlyTeamsArchersFormations, enemyTeamsArchersFormations);
            }
            else if (formation.QuerySystem.IsCavalryFormation)
            {
                cavalryCaptains.Add(formation.Captain);
                AddFormationToTeamList(formation, friendlyTeamsCavalryFormations, enemyTeamsCavalryFormations);
            }
            else if (formation.QuerySystem.IsRangedCavalryFormation)
            {
                horseArchersCaptains.Add(formation.Captain);
                AddFormationToTeamList(formation, friendlyTeamsHorseArchersFormations, enemyTeamsHorseArchersFormations);
            }
        }

        private void AddFormationToTeamList(Formation formation, List<Formation> friendlyFormations, List<Formation> enemyFormations)
        {
            if (formation.Team.IsPlayerAlly)
                friendlyFormations.Add(formation);
            else
                enemyFormations.Add(formation);
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow blow)
        {
            base.OnAgentRemoved(affectedAgent, affectorAgent, agentState, blow);

            if (!settings.EnableThisModification || Mission.Current == null || (!Mission.Current.IsFieldBattle && !Mission.Current.IsSiegeBattle) ||
                affectedAgent == null || affectorAgent == null || affectedAgent == affectorAgent) return;

            if (!affectedAgent.IsHero)
                return;

            if (affectorAgent.IsHero)
                HeroKnockedDownAgent(affectorAgent, affectedAgent);
            else
                SimpleTroopKnockedDownAgent(affectorAgent, affectedAgent);

            if (settings.LoggingEnabled)
                MissionUtilities.DisplayKnockdownMessage(affectorAgent, affectedAgent);
        }

        private void HeroKnockedDownAgent(Agent attackerAgent, Agent victimAgent)
        {
            if (MissionUtilities.IsAgentGeneral(attackerAgent))
                GeneralKnockedDownAgent(attackerAgent, victimAgent);
            else if (MissionUtilities.IsAgentCaptain(attackerAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
                CaptainKnockedDownAgent(attackerAgent, victimAgent);
            else
                UnassignedHeroKnockedDownAgent(attackerAgent, victimAgent);
        }

        private void GeneralKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
        {
            MissionUtilities.SetWantsToYellForTeam(affectorAgent.Team);

            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenGeneralHeroKillsGeneralHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.UpdateFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenGeneralHeroKillsCaptainHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectorAgent, rangeForTroopsToReactToGeneralHeroFall, -settings.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void CaptainKnockedDownAgent(Agent attackerAgent, Agent victimAgent)
        {
            MissionUtilities.SetWantsToYellForFormation(troopsOfFormationCaptains[attackerAgent]);

            if (MissionUtilities.IsAgentGeneral(victimAgent))
            {
                MissionUtilities.UpdateTeamMorale(victimAgent.Team, -settings.MoraleChangeWhenCaptainHeroKillsGeneralHero);
                ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(victimAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.UpdateFormationMorale(troopsOfFormationCaptains, victimAgent, -settings.MoraleChangeWhenCaptainHeroKillsCaptainHero);
                ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.UpdateMoraleForNearbyAgents(victimAgent.Team, attackerAgent, rangeForTroopsToReactToUnassignedHeroFall + 10, -settings.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
                ShowOnScreenNotification(attackerAgent, victimAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void UnassignedHeroKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
        {
            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, rangeForTroopsToReactToGeneralHeroFall + 5);
                MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, rangeForTroopsToReactToCaptainHeroFall + 5);
                MissionUtilities.UpdateFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, rangeForTroopsToReactToUnassignedHeroFall + 5);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, rangeForTroopsToReactToUnassignedHeroFall, -settings.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void SimpleTroopKnockedDownAgent(Agent affectorAgent, Agent affectedAgent)
        {
            if (MissionUtilities.IsAgentGeneral(affectedAgent))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, rangeForTroopsToReactToGeneralHeroFall);
                MissionUtilities.UpdateTeamMorale(affectedAgent.Team, -settings.MoraleChangeWhenTroopKillsGeneralHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenGeneralFalls);
            }
            else if (MissionUtilities.IsAgentCaptain(affectedAgent, friendlyInfantryCaptains, friendlyArchersCaptains, friendlyCavalryCaptains, friendlyHorseArchersCaptains, enemyInfantryCaptains, enemyArchersCaptains, enemyCavalryCaptains, enemyHorseArchersCaptains))
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, rangeForTroopsToReactToCaptainHeroFall);
                MissionUtilities.UpdateFormationMorale(troopsOfFormationCaptains, affectedAgent, -settings.MoraleChangeWhenTroopKillsCaptainHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenCaptainFalls);
            }
            else
            {
                MissionUtilities.SetWantsToYellInRange(affectorAgent, rangeForTroopsToReactToUnassignedHeroFall);
                MissionUtilities.UpdateMoraleForNearbyAgents(affectedAgent.Team, affectedAgent, rangeForTroopsToReactToUnassignedHeroFall, -settings.MoraleChangeWhenTroopKillsUnassignedHero);
                ShowOnScreenNotification(affectorAgent, affectedAgent, MissionUtilities.DisplayQuickInformationMessageWhenUnassignedHeroFalls);
            }
        }

        private void ShowOnScreenNotification(Agent affectorAgent, Agent affectedAgent, Action<Agent, Agent> displayNotification)
        {
            if (settings.ShowOnScreenNotifications)
                displayNotification(affectorAgent, affectedAgent);
        }
    }
}