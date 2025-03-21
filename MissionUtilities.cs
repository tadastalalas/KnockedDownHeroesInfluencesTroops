using System.Collections.Generic;
using System.Linq;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public static class MissionUtilities
    {
        public static void LogTeamAndFormationCounts(int totalTeamsCount, int friendlyTeamsCount, int enemyTeamsCount, List<Formation> friendlyTeamsFormations, List<Formation> enemyTeamsFormations, List<Formation> friendlyTeamsInfantryFormations, List<Formation> friendlyTeamsArchersFormations, List<Formation> friendlyTeamsCavalryFormations, List<Formation> friendlyTeamsHorseArchersFormations, List<Formation> enemyTeamsInfantryFormations, List<Formation> enemyTeamsArchersFormations, List<Formation> enemyTeamsCavalryFormations, List<Formation> enemyTeamsHorseArchersFormations)
        {
            InformationManager.DisplayMessage(new InformationMessage($"Teams count: {totalTeamsCount}", Colors.Magenta));
            InformationManager.DisplayMessage(new InformationMessage($"Friendly teams count: {friendlyTeamsCount}", Colors.Green));
            InformationManager.DisplayMessage(new InformationMessage($"Enemy teams count: {enemyTeamsCount}", Colors.Red));
            InformationManager.DisplayMessage(new InformationMessage($"Friendly formations count: {friendlyTeamsFormations.Count}", Colors.Green));
            InformationManager.DisplayMessage(new InformationMessage($"Enemy formations count: {enemyTeamsFormations.Count}", Colors.Red));
            InformationManager.DisplayMessage(new InformationMessage($"Friendly infantry formations count: {friendlyTeamsInfantryFormations.Count}", Colors.Green));
            InformationManager.DisplayMessage(new InformationMessage($"Friendly archers formations count: {friendlyTeamsArchersFormations.Count}", Colors.Green));
            InformationManager.DisplayMessage(new InformationMessage($"Friendly cavalry formations count: {friendlyTeamsCavalryFormations.Count}", Colors.Green));
            InformationManager.DisplayMessage(new InformationMessage($"Friendly horse archers formations count: {friendlyTeamsHorseArchersFormations.Count}", Colors.Green));
            InformationManager.DisplayMessage(new InformationMessage($"Enemy infantry formations count: {enemyTeamsInfantryFormations.Count}", Colors.Red));
            InformationManager.DisplayMessage(new InformationMessage($"Enemy archers formations count: {enemyTeamsArchersFormations.Count}", Colors.Red));
            InformationManager.DisplayMessage(new InformationMessage($"Enemy cavalry formations count: {enemyTeamsCavalryFormations.Count}", Colors.Red));
            InformationManager.DisplayMessage(new InformationMessage($"Enemy horse archers formations count: {enemyTeamsHorseArchersFormations.Count}", Colors.Red));
        }

        public static void LogCaptainMessages(List<(string message, Color color)> captainLogMessages)
        {
            foreach (var (message, color) in captainLogMessages)
            {
                InformationManager.DisplayMessage(new InformationMessage(message, color));
            }
            captainLogMessages.Clear();
        }

        public static void ChangeMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange, bool setWantsToYell = false)
        {
            foreach (var agent in team.ActiveAgents)
            {
                if (agent.Position.Distance(referenceAgent.Position) < range)
                {
                    if (setWantsToYell)
                        agent.SetWantsToYell();
                    agent.ChangeMorale(moraleChange);
                }
            }
        }

        public static void ModifyTeamMorale(Team affectedTeam, float moraleChange)
        {
            foreach (var agent in affectedTeam.ActiveAgents)
                agent?.ChangeMorale(moraleChange);
        }

        public static void ModifyFormationMorale(Dictionary<Agent, List<Agent>> troopsOfFormationCaptains, Agent formationCaptain, float moraleChange)
        {
            foreach (var agent in troopsOfFormationCaptains[formationCaptain])
                agent?.ChangeMorale(moraleChange);
        }

        public static void DisplayKnockdownMessage(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectorAgent == null || affectedAgent == null || affectorAgent.Team == null || affectedAgent.Team == null)
                return;

            string affectorName = affectorAgent.Name?.ToString() ?? "Unknown";
            string affectedName = affectedAgent.Name?.ToString() ?? "Unknown";
            Color messageColor = affectorAgent.Team.IsPlayerAlly ? Colors.Green : Colors.Red;

            InformationManager.DisplayMessage(new InformationMessage($"{affectorName} knocked down {affectedName}.", messageColor));
        }

        public static void DisplayQuickInformationMessageWhenGeneralFalls(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent.Team.IsPlayerTeam || affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_zSNYnN}Your stupid general is dead! Nobody will save you now!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_VrpEkb}Enemy's general is dead! We will win this war!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
        }

        public static void DisplayQuickInformationMessageWhenCaptainFalls(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent.Team.IsPlayerTeam || affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_44PmYz}Your idiot captain is dead! Give up, you dogs!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_GeOvFE}Enemy's captain is dead! They stand no chance against us!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
        }

        public static void DisplayQuickInformationMessageWhenUnassignedHeroFalls(Agent affectorAgent, Agent affectedAgent)
        {
            if (affectedAgent.Team.IsPlayerTeam || affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_XlhfeB}Another one of your lapdogs is dead!"), 2000, affectorAgent.Character, "event:/ui/notification/death");
            if (!affectedAgent.Team.IsPlayerTeam && !affectedAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject("{=KDHIT_SLjiPR}We have slain another one of their lords!"), 2000, affectorAgent.Character, "event:/ui/notification/levelup");
        }

        public static void SetWantsToYellForTeam(Team team)
        {
            foreach (Agent agent in team.ActiveAgents)
                agent?.SetWantsToYell();
        }

        public static void SetWantsToYellForFormation(List<Agent> troopsInFormation)
        {
            foreach (Agent agent in troopsInFormation)
                agent?.SetWantsToYell();
        }

        public static bool IsAgentGeneral(Agent agent) => agent == agent.Team.GeneralAgent;

        public static bool IsAgentCaptain(Agent agent, List<Agent> friendlyInfantryCaptains, List<Agent> friendlyArchersCaptains, List<Agent> friendlyCavalryCaptains, List<Agent> friendlyHorseArchersCaptains, List<Agent> enemyInfantryCaptains, List<Agent> enemyArchersCaptains, List<Agent> enemyCavalryCaptains, List<Agent> enemyHorseArchersCaptains)
        {
            if (friendlyInfantryCaptains.Contains(agent)) return true;
            if (friendlyArchersCaptains.Contains(agent)) return true;
            if (friendlyCavalryCaptains.Contains(agent)) return true;
            if (friendlyHorseArchersCaptains.Contains(agent)) return true;
            if (enemyInfantryCaptains.Contains(agent)) return true;
            if (enemyArchersCaptains.Contains(agent)) return true;
            if (enemyCavalryCaptains.Contains(agent)) return true;
            if (enemyHorseArchersCaptains.Contains(agent)) return true;
            return false;
        }

        public static Agent? FindCaptainForAgent(Agent agent, List<Agent> friendlyInfantryCaptains, List<Agent> friendlyArchersCaptains, List<Agent> friendlyCavalryCaptains, List<Agent> friendlyHorseArchersCaptains, List<Agent> enemyInfantryCaptains, List<Agent> enemyArchersCaptains, List<Agent> enemyCavalryCaptains, List<Agent> enemyHorseArchersCaptains)
        {
            if (friendlyInfantryCaptains.Contains(agent)) return agent;
            if (friendlyArchersCaptains.Contains(agent)) return agent;
            if (friendlyCavalryCaptains.Contains(agent)) return agent;
            if (friendlyHorseArchersCaptains.Contains(agent)) return agent;
            if (enemyInfantryCaptains.Contains(agent)) return agent;
            if (enemyArchersCaptains.Contains(agent)) return agent;
            if (enemyCavalryCaptains.Contains(agent)) return agent;
            if (enemyHorseArchersCaptains.Contains(agent)) return agent;
            return null;
        }

        public static void ClearAllLists(List<Formation> enemyTeamsFormations, List<Formation> friendlyTeamsFormations, List<Formation> friendlyTeamsInfantryFormations, List<Formation> friendlyTeamsArchersFormations, List<Formation> friendlyTeamsCavalryFormations, List<Formation> friendlyTeamsHorseArchersFormations, List<Formation> enemyTeamsInfantryFormations, List<Formation> enemyTeamsArchersFormations, List<Formation> enemyTeamsCavalryFormations, List<Formation> enemyTeamsHorseArchersFormations, List<Agent> friendlyInfantryCaptains, List<Agent> friendlyArchersCaptains, List<Agent> friendlyCavalryCaptains, List<Agent> friendlyHorseArchersCaptains, List<Agent> enemyInfantryCaptains, List<Agent> enemyArchersCaptains, List<Agent> enemyCavalryCaptains, List<Agent> enemyHorseArchersCaptains)
        {
            enemyTeamsFormations.Clear();
            friendlyTeamsFormations.Clear();
            friendlyTeamsInfantryFormations.Clear();
            friendlyTeamsArchersFormations.Clear();
            friendlyTeamsCavalryFormations.Clear();
            friendlyTeamsHorseArchersFormations.Clear();
            enemyTeamsInfantryFormations.Clear();
            enemyTeamsArchersFormations.Clear();
            enemyTeamsCavalryFormations.Clear();
            enemyTeamsHorseArchersFormations.Clear();
            friendlyInfantryCaptains.Clear();
            friendlyArchersCaptains.Clear();
            friendlyCavalryCaptains.Clear();
            friendlyHorseArchersCaptains.Clear();
            enemyInfantryCaptains.Clear();
            enemyArchersCaptains.Clear();
            enemyCavalryCaptains.Clear();
            enemyHorseArchersCaptains.Clear();
        }
    }
}