using System;
using System.Collections.Generic;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public static class MissionUtilities
    {
        private static MCMSettings? _fallbackSettings;
        private static MCMSettings Settings => AttributeGlobalSettings<MCMSettings>.Instance ?? (_fallbackSettings ??= new MCMSettings());

        private static readonly Queue<Agent> CheerQueue = new();
        private static float _cheerBatchTimer;
        private const int CheerBatchSize = 10;
        private const float CheerBatchInterval = 0.1f;
        private static bool _cheeringInProgress;
        
        private const string GeneralFallReactionSoundMale = "kdhit_general_fall_reaction_male";
        private const string GeneralFallReactionSoundFemale = "kdhit_general_fall_reaction_female";
        private const string CaptainFallReactionSoundMale = "kdhit_captain_fall_reaction_male";
        private const string CaptainFallReactionSoundFemale = "kdhit_captain_fall_reaction_female";

        private static readonly Dictionary<string, int> FallReactionSoundIdCache = new();

        private const string SoundFriendlyFalls = "event:/ui/notification/death";
        private const string SoundEnemyFalls = "event:/ui/notification/levelup";

        private static string ResolveSound(bool isFriendly)
        {
            if (Settings.DisableKnockdownSounds)
                return string.Empty;
            return isFriendly ? SoundFriendlyFalls : SoundEnemyFalls;
        }
        
        public static void PlayHeroFallReactionVoiceLine(Agent victimAgent, bool victimIsGeneral)
        {
            if (!Settings.PlayHeroFallReactionVoiceLines || Mission.Current == null)
                return;

            Agent? mainAgent = Agent.Main;
            if (mainAgent?.Team == null || !mainAgent.IsActive() || mainAgent == victimAgent)
                return;

            if (victimAgent.Team?.Side != mainAgent.Team.Side)
                return;

            string soundName = victimIsGeneral
                ? (mainAgent.IsFemale ? GeneralFallReactionSoundFemale : GeneralFallReactionSoundMale)
                : (mainAgent.IsFemale ? CaptainFallReactionSoundFemale : CaptainFallReactionSoundMale);

            if (!FallReactionSoundIdCache.TryGetValue(soundName, out int soundId))
            {
                soundId = SoundEvent.GetEventIdFromString(soundName);
                if (soundId >= 0)
                    FallReactionSoundIdCache[soundName] = soundId;
            }

            if (soundId < 0)
                return;

            Mission.Current.MakeSound(soundId, mainAgent.Position, false, true, -1, -1);
        }

        public static void UpdateMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange, bool setWantsToYell = false)
        {
            int troopCount = 0;

            foreach (var agent in team.ActiveAgents)
            {
                if (agent.Position.Distance(referenceAgent.Position) < range)
                {
                    agent.ChangeMorale(moraleChange);
                    troopCount++;
                }
            }

            if (!Settings.LoggingEnabled)
                return;
            
            string logMessage = $"Number of troops affected by morale change and yell in range: {troopCount}";

            InformationManager.DisplayMessage(moraleChange > 0
                ? new InformationMessage(logMessage, Colors.Green)
                : new InformationMessage(logMessage, Colors.Red));
        }

        public static void UpdateTeamMorale(Team affectedTeam, float moraleChange)
        {
            int troopCount = 0;

            foreach (var agent in affectedTeam.ActiveAgents)
            {
                agent.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (!Settings.LoggingEnabled)
                return;
            
            string logMessage = $"Number of troops affected by morale change in the team: {troopCount}";
            InformationManager.DisplayMessage(moraleChange > 0
                ? new InformationMessage(logMessage, Colors.Green)
                : new InformationMessage(logMessage, Colors.Red));
        }

        public static void UpdateFormationMorale(Dictionary<Agent, List<Agent>> troopsOfFormationCaptains, Agent formationCaptain, float moraleChange)
        {
            int troopCount = 0;

            foreach (var agent in troopsOfFormationCaptains[formationCaptain])
            {
                agent.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (!Settings.LoggingEnabled)
                return;
            
            string logMessage = $"Number of troops affected by morale change in formation: {troopCount}";

            InformationManager.DisplayMessage(moraleChange > 0
                ? new InformationMessage(logMessage, Colors.Green)
                : new InformationMessage(logMessage, Colors.Red));
        }

        public static void SetWantsToYellForTeam(Team team)
        {
            EnqueueAgentsToCheer(team.ActiveAgents);
        }

        public static void SetWantsToYellForFormation(List<Agent> troopsInFormation)
        {
            EnqueueAgentsToCheer(troopsInFormation);
        }

        public static void SetWantsToYellInRange(Agent attackerAgent, float range)
        {
            if (attackerAgent.Team?.ActiveAgents == null)
                return;

            EnqueueAgentsToCheer(
                attackerAgent.Team.ActiveAgents,
                agent => agent.Position.Distance(attackerAgent.Position) < range
            );
        }

        private static void EnqueueAgentsToCheer(IEnumerable<Agent> agents, Func<Agent, bool>? filter = null)
        {
            int enqueuedCount = 0;
            foreach (var agent in agents)
            {
                if (agent == null)
                    continue;
                if (filter != null && !filter(agent))
                    continue;
                CheerQueue.Enqueue(agent);
                enqueuedCount++;
            }
            if (enqueuedCount > 0)
                _cheeringInProgress = true;

            // Logging: only log when something is actually enqueued
            if (!Settings.LoggingEnabled || enqueuedCount <= 0)
                return;
            
            string logMessage = $"Number of troops queued to cheer: {enqueuedCount}";
            InformationManager.DisplayMessage(new InformationMessage(logMessage, Colors.Yellow));
        }

        public static void ProcessCheerQueue(float dt)
        {
            if (!_cheeringInProgress)
                return;

            _cheerBatchTimer += dt;
            if (_cheerBatchTimer < CheerBatchInterval)
                return;

            int count = 0;
            while (CheerQueue.Count > 0 && count < CheerBatchSize)
            {
                var agent = CheerQueue.Dequeue();
                if (!agent.IsActive()) 
                    continue;
                
                agent.SetWantsToYell();
                count++;
            }

            _cheerBatchTimer = 0f;

            if (CheerQueue.Count == 0)
                _cheeringInProgress = false;
        }

        public static bool IsAgentGeneral(Agent agent) => agent.Team?.GeneralAgent == agent;

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

        public static void DisplayKnockdownMessage(Agent? attackerAgent, Agent? victimAgent)
        {
            if (attackerAgent == null || victimAgent == null || attackerAgent.Team == null || victimAgent.Team == null)
                return;

            string affectorName = attackerAgent.Name ?? "Unknown";
            string affectedName = victimAgent.Name ?? "Unknown";
            Color messageColor = attackerAgent.Team.IsPlayerAlly ? Colors.Yellow : Colors.Red;

            InformationManager.DisplayMessage(new InformationMessage($"{affectorName} knocked down {affectedName}.", messageColor));
        }

        public static void DisplayQuickInformationMessageWhenGeneralFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(Settings.friendlyGeneralFallenNotification), 2000, attackerAgent.Character, null, ResolveSound(true));
            if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(Settings.enemyGeneralFallenNotification), 2000, attackerAgent.Character, null, ResolveSound(false));
        }

        public static void DisplayQuickInformationMessageWhenCaptainFalls(Agent attackerAgent, Agent victimAgent)
        {
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
                MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent.Character, null, ResolveSound(true));
            }
            else if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
            {
                string message = formationType switch
                {
                    "Infantry" => Settings.enemyInfantryCaptainFallenNotification,
                    "Archers" => Settings.enemyRangedCaptainFallenNotification,
                    "Cavalry" => Settings.enemyCavalryCaptainFallenNotification,
                    "Horse Archers" => Settings.enemyHorseArchersCaptainFallenNotification,
                    _ => "Enemy's captain has fallen! Good fight!"
                };
                MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent.Character, null, ResolveSound(false));
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

            if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(Settings.friendlyUnassignedHeroFallenNotification), 2000, attackerAgent.Character, null, ResolveSound(true));
            if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
                MBInformationManager.AddQuickInformation(new TextObject(Settings.enemyUnassignedHeroFallenNotification), 2000, attackerAgent.Character, null, ResolveSound(false));
        }

        public static void ShowLogs(List<(string message, Color color)> captainLogMessages)
        {
            foreach (var (message, color) in captainLogMessages)
            {
                InformationManager.DisplayMessage(new InformationMessage(message, color));
            }
            captainLogMessages.Clear();
        }
        
        public static void ResetCheerState()
        {
            CheerQueue.Clear();
            _cheerBatchTimer = 0f;
            _cheeringInProgress = false;
        }
    }
}