using System.Collections.Generic;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public static class MissionUtilities
    {
        private static readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        public static void UpdateMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange, bool setWantsToYell = false)
        {
            int troopCount = 0;

            foreach (var agent in team.ActiveAgents)
            {
                if (agent.Position.Distance(referenceAgent.Position) < range)
                {
                    agent?.ChangeMorale(moraleChange);
                    troopCount++;
                }
            }

            if (settings.LoggingEnabled)
            {
                string logMessage = $"Number of troops affected by morale change and yell in range: {troopCount}";

                if (moraleChange > 0)
                    InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Green));
                else
                    InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Red));
            }
        }

        public static void UpdateTeamMorale(Team affectedTeam, float moraleChange)
        {
            int troopCount = 0;

            foreach (var agent in affectedTeam.ActiveAgents)
            {
                agent?.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (settings.LoggingEnabled)
            {
                string logMessage = $"Number of troops affected by morale change in the team: {troopCount}";

                if (moraleChange > 0)
                    InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Green));
                else
                    InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Red));
            }
                
        }

        public static void UpdateFormationMorale(Dictionary<Agent, List<Agent>> troopsOfFormationCaptains, Agent formationCaptain, float moraleChange)
        {
            int troopCount = 0;

            foreach (var agent in troopsOfFormationCaptains[formationCaptain])
            {
                agent?.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (settings.LoggingEnabled)
            {
                string logMessage = $"Number of troops affected by morale change in formation: {troopCount}";

                if (moraleChange > 0)
                    InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Green));
                else
                    InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Red));
            }
        }

        public static void SetWantsToYellForTeam(Team team)
        {
            int troopCount = 0;

            foreach (Agent agent in team.ActiveAgents)
            {
                agent?.SetWantsToYell();
                troopCount++;
            }

            if (settings.LoggingEnabled)
            {
                string logMessage = $"Number of troops yells in the team: {troopCount}";
                InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Yellow));
            }
        }

        public static void SetWantsToYellForFormation(List<Agent> troopsInFormation)
        {
            int troopCount = 0;

            foreach (Agent agent in troopsInFormation)
            {
                agent?.SetWantsToYell();
                troopCount++;
            }

            if (settings.LoggingEnabled)
            {
                string logMessage = $"Number of troops yells in formation: {troopCount}";
                InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.White));
            }
        }

        public static void SetWantsToYellInRange(Agent attackerAgent, float range)
        {
            if (attackerAgent?.Team?.ActiveAgents == null)
                return;

            int troopCount = 0;

            foreach (var agent in attackerAgent.Team.ActiveAgents)
            {
                if (agent == null)
                    continue;

                if (agent.Position.Distance(attackerAgent.Position) < range)
                {
                    agent?.SetWantsToYell();
                    troopCount++;
                }
            }

            if (settings.LoggingEnabled)
            {
                string logMessage = $"Number of troops yell in range: {troopCount}";
                InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.White));
            }
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

        public static void DisplayKnockdownMessage(Agent attackerAgent, Agent victimAgent)
        {
            if (attackerAgent == null || victimAgent == null || attackerAgent.Team == null || victimAgent.Team == null)
                return;

            string affectorName = attackerAgent.Name?.ToString() ?? "Unknown";
            string affectedName = victimAgent.Name?.ToString() ?? "Unknown";
            Color messageColor = attackerAgent.Team.IsPlayerAlly ? Colors.Yellow : Colors.Red;

            InformationManager.DisplayMessage(new InformationMessage($"{affectorName} knocked down {affectedName}.", messageColor));
        }

        public static void DisplayQuickInformationMessageWhenGeneralFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(settings.friendlyGeneralFallenNotification), 2000, attackerAgent.Character, "event:/ui/notification/death");
            if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(settings.enemyGeneralFallenNotification), 2000, attackerAgent.Character, "event:/ui/notification/levelup");
        }

        public static void DisplayQuickInformationMessageWhenCaptainFalls(Agent attackerAgent, Agent victimAgent)
        {
            string formationType = GetFormationType(victimAgent);

            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
            {
                string message = formationType switch
                {
                    "Infantry" => settings.friendlyInfantryCaptainFallenNotification,
                    "Archers" => settings.friendlyRangedCaptainFallenNotification,
                    "Cavalry" => settings.friendlyCavalryCaptainFallenNotification,
                    "Horse Archers" => settings.friendlyHorseArchersCaptainFallenNotification,
                    _ => "Your captain has fallen! Give up!"
                };
                MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent.Character, "event:/ui/notification/death");
            }
            else if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
            {
                string message = formationType switch
                {
                    "Infantry" => settings.enemyInfantryCaptainFallenNotification,
                    "Archers" => settings.enemyRangedCaptainFallenNotification,
                    "Cavalry" => settings.enemyCavalryCaptainFallenNotification,
                    "Horse Archers" => settings.enemyHorseArchersCaptainFallenNotification,
                    _ => "Enemy's captain has fallen! Good fight!"
                };
                MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent.Character, "event:/ui/notification/levelup");
            }
        }

        private static string GetFormationType(Agent agent)
        {
            if (agent.Formation != null)
            {
                if (agent.Formation.QuerySystem.IsInfantryFormation)
                    return "Infantry";
                if (agent.Formation.QuerySystem.IsRangedFormation)
                    return "Archers";
                if (agent.Formation.QuerySystem.IsCavalryFormation)
                    return "Cavalry";
                if (agent.Formation.QuerySystem.IsRangedCavalryFormation)
                    return "Horse Archers";
            }
            return "Unknown";
        }
        public static void DisplayQuickInformationMessageWhenUnassignedHeroFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(settings.friendlyUnassignedHeroFallenNotification), 2000, attackerAgent.Character, "event:/ui/notification/death");
            if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(settings.enemyUnassignedHeroFallenNotification), 2000, attackerAgent.Character, "event:/ui/notification/levelup");
        }

        public static void ShowLogs(List<(string message, Color color)> captainLogMessages)
        {
            foreach (var (message, color) in captainLogMessages)
            {
                InformationManager.DisplayMessage(new InformationMessage(message, color));
            }
            captainLogMessages.Clear();
        }
    }
}