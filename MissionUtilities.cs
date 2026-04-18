using System;
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
        // Always re-resolve from MCM so runtime setting changes take effect.
        // Falls back to a single throwaway instance if MCM hasn't initialized yet.
        private static readonly MCMSettings _fallback = new();
        private static MCMSettings Settings => AttributeGlobalSettings<MCMSettings>.Instance ?? _fallback;

        private const string SoundFriendlyHeroDown = "event:/ui/notification/death";
        private const string SoundEnemyHeroDown = "event:/ui/notification/levelup";

        private static readonly Queue<Agent> _cheerQueue = new();
        private static float _cheerBatchTimer = 0f;
        private static int _cheerBatchSize = 10; // Number of agents to cheer per batch
        private static float _cheerBatchInterval = 0.1f; // Seconds between batches
        private static bool _cheeringInProgress = false;

        /// <summary>
        /// Resets static cross-mission state so the next mission starts clean.
        /// </summary>
        public static void ResetCheerQueue()
        {
            _cheerQueue.Clear();
            _cheerBatchTimer = 0f;
            _cheeringInProgress = false;
        }

        private static string GetSoundEvent(string defaultEvent)
            => Settings.DisableKnockdownSounds ? string.Empty : defaultEvent;

        public static void UpdateMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange, bool setWantsToYell = false)
        {
            if (team == null || referenceAgent == null)
                return;

            int troopCount = 0;

            foreach (var agent in team.ActiveAgents)
            {
                if (agent == null)
                    continue;
                if (agent.Position.Distance(referenceAgent.Position) < range)
                {
                    agent.ChangeMorale(moraleChange);
                    troopCount++;
                }
            }

            if (Settings.LoggingEnabled)
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
            if (affectedTeam == null)
                return;

            int troopCount = 0;

            foreach (var agent in affectedTeam.ActiveAgents)
            {
                if (agent == null)
                    continue;
                agent.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (Settings.LoggingEnabled)
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
            if (formationCaptain == null || troopsOfFormationCaptains == null)
                return;

            // Guard B: avoid KeyNotFoundException if captain isn't in the cached map yet
            if (!troopsOfFormationCaptains.TryGetValue(formationCaptain, out var troops) || troops == null)
                return;

            int troopCount = 0;

            foreach (var agent in troops)
            {
                if (agent == null)
                    continue;
                agent.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (Settings.LoggingEnabled)
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
            if (team == null)
                return;
            EnqueueAgentsToCheer(team.ActiveAgents);
        }

        public static void SetWantsToYellForFormation(List<Agent> troopsInFormation)
        {
            if (troopsInFormation == null)
                return;
            EnqueueAgentsToCheer(troopsInFormation);
        }

        public static void SetWantsToYellInRange(Agent attackerAgent, float range)
        {
            if (attackerAgent?.Team?.ActiveAgents == null)
                return;

            EnqueueAgentsToCheer(
                attackerAgent.Team.ActiveAgents,
                agent => agent.Position.Distance(attackerAgent.Position) < range
            );
        }

        public static void EnqueueAgentsToCheer(IEnumerable<Agent> agents, Func<Agent, bool>? filter = null)
        {
            int enqueuedCount = 0;
            foreach (var agent in agents)
            {
                if (agent == null)
                    continue;
                if (filter != null && !filter(agent))
                    continue;
                _cheerQueue.Enqueue(agent);
                enqueuedCount++;
            }
            if (enqueuedCount > 0)
                _cheeringInProgress = true;

            // Logging: only log when something is actually enqueued
            if (Settings.LoggingEnabled && enqueuedCount > 0)
            {
                string logMessage = $"Number of troops queued to cheer: {enqueuedCount}";
                InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Yellow));
            }
        }

        public static void ProcessCheerQueue(float dt)
        {
            if (!_cheeringInProgress)
                return;

            _cheerBatchTimer += dt;
            if (_cheerBatchTimer < _cheerBatchInterval)
                return;

            int count = 0;
            while (_cheerQueue.Count > 0 && count < _cheerBatchSize)
            {
                var agent = _cheerQueue.Dequeue();
                // Agent may have been removed since being queued
                if (agent != null && agent.IsActive())
                    agent.SetWantsToYell();
                count++;
            }

            _cheerBatchTimer = 0f;

            if (_cheerQueue.Count == 0)
                _cheeringInProgress = false;
        }

        // Guard C: agent.Team may be null after removal
        public static bool IsAgentGeneral(Agent agent)
            => agent != null && agent.Team != null && agent == agent.Team.GeneralAgent;

        public static bool IsAgentCaptain(Agent agent, List<Agent> friendlyInfantryCaptains, List<Agent> friendlyArchersCaptains, List<Agent> friendlyCavalryCaptains, List<Agent> friendlyHorseArchersCaptains, List<Agent> enemyInfantryCaptains, List<Agent> enemyArchersCaptains, List<Agent> enemyCavalryCaptains, List<Agent> enemyHorseArchersCaptains)
        {
            if (agent == null) return false;
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
            if (victimAgent?.Team == null || attackerAgent == null)
                return;

            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(Settings.friendlyGeneralFallenNotification), 2000, attackerAgent.Character, null, GetSoundEvent(SoundFriendlyHeroDown));
            else
                MBInformationManager.AddQuickInformation(new TextObject(Settings.enemyGeneralFallenNotification), 2000, attackerAgent.Character, null, GetSoundEvent(SoundEnemyHeroDown));
        }

        public static void DisplayQuickInformationMessageWhenCaptainFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (victimAgent?.Team == null || attackerAgent == null)
                return;

            string formationType = GetFormationType(victimAgent);

            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
            {
                string message = formationType switch
                {
                    "Infantry" => Settings.friendlyInfantryCaptainFallenNotification,
                    "Archers" => Settings.friendlyRangedCaptainFallenNotification,
                    "Cavalry" => Settings.friendlyCavalryCaptainFallenNotification,
                    "Horse Archers" => Settings.friendlyHorseArchersCaptainFallenNotification,
                    _ => "Your captain has fallen! Give up!"
                };
                MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent.Character, null, GetSoundEvent(SoundFriendlyHeroDown));
            }
            else
            {
                string message = formationType switch
                {
                    "Infantry" => Settings.enemyInfantryCaptainFallenNotification,
                    "Archers" => Settings.enemyRangedCaptainFallenNotification,
                    "Cavalry" => Settings.enemyCavalryCaptainFallenNotification,
                    "Horse Archers" => Settings.enemyHorseArchersCaptainFallenNotification,
                    _ => "Enemy's captain has fallen! Good fight!"
                };
                MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent.Character, null, GetSoundEvent(SoundEnemyHeroDown));
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
            if (Settings.HideUnassignedHeroNotifications)
                return;

            if (victimAgent?.Team == null || attackerAgent == null)
                return;

            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(Settings.friendlyUnassignedHeroFallenNotification), 2000, attackerAgent.Character, null, GetSoundEvent(SoundFriendlyHeroDown));
            else
                MBInformationManager.AddQuickInformation(new TextObject(Settings.enemyUnassignedHeroFallenNotification), 2000, attackerAgent.Character, null, GetSoundEvent(SoundEnemyHeroDown));
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