using System;
using System.Collections.Generic;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public enum FormationCategory
    {
        Unknown,
        Infantry,
        Archers,
        Cavalry,
        HorseArchers
    }

    public static class MissionUtilities
    {
        private static readonly MCMSettings settings = AttributeGlobalSettings<MCMSettings>.Instance ?? new MCMSettings();

        private static readonly Queue<Agent> _cheerQueue = new();
        private static float _cheerBatchTimer = 0f;
        private const int CheerBatchSize = 10;
        private const float CheerBatchInterval = 0.1f;
        private static bool _cheeringInProgress = false;

        private const string SoundFriendlyFalls = "event:/ui/notification/death";
        private const string SoundEnemyFalls = "event:/ui/notification/levelup";

        public static void ResetCheerState()
        {
            _cheerQueue.Clear();
            _cheerBatchTimer = 0f;
            _cheeringInProgress = false;
        }

        public static bool IsAgentGeneral(Agent agent)
            => agent != null && agent.Team != null && agent.Team.GeneralAgent == agent;

        public static bool IsAgentCaptain(Agent agent)
            => agent?.Formation != null && agent.Formation.Captain == agent;

        public static FormationCategory GetFormationCategory(Formation formation)
        {
            if (formation == null || formation.QuerySystem == null)
                return FormationCategory.Unknown;
            if (formation.QuerySystem.IsInfantryFormation) return FormationCategory.Infantry;
            if (formation.QuerySystem.IsRangedFormation) return FormationCategory.Archers;
            if (formation.QuerySystem.IsCavalryFormation) return FormationCategory.Cavalry;
            if (formation.QuerySystem.IsRangedCavalryFormation) return FormationCategory.HorseArchers;
            return FormationCategory.Unknown;
        }

        public static void UpdateMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange)
        {
            if (team == null || referenceAgent == null)
                return;

            float rangeSq = range * range;
            Vec3 refPos = referenceAgent.Position;
            int troopCount = 0;

            foreach (var agent in team.ActiveAgents)
            {
                if (agent == null)
                    continue;
                if (agent.Position.DistanceSquared(refPos) < rangeSq)
                {
                    agent.ChangeMorale(moraleChange);
                    troopCount++;
                }
            }

            if (settings.LoggingEnabled)
                LogMoraleChange($"Number of troops affected by morale change and yell in range: {troopCount}", moraleChange);
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

            if (settings.LoggingEnabled)
                LogMoraleChange($"Number of troops affected by morale change in the team: {troopCount}", moraleChange);
        }

        public static void UpdateFormationMorale(Formation formation, float moraleChange)
        {
            if (formation == null || formation.Team == null)
                return;

            int troopCount = 0;

            foreach (var agent in formation.Team.ActiveAgents)
            {
                if (agent == null || agent.Formation != formation)
                    continue;
                agent.ChangeMorale(moraleChange);
                troopCount++;
            }

            if (settings.LoggingEnabled)
                LogMoraleChange($"Number of troops affected by morale change in formation: {troopCount}", moraleChange);
        }

        private static void LogMoraleChange(string message, float moraleChange)
        {
            Color color = moraleChange > 0 ? Colors.Green : Colors.Red;
            InformationManager.DisplayMessage(new InformationMessage(message, color));
        }

        public static void SetWantsToYellForTeam(Team team)
        {
            if (team == null)
                return;
            EnqueueAgentsToCheer(team.ActiveAgents);
        }

        public static void SetWantsToYellForFormation(Formation formation)
        {
            if (formation == null || formation.Team == null)
                return;
            EnqueueAgentsToCheer(formation.Team.ActiveAgents, a => a.Formation == formation);
        }

        public static void SetWantsToYellInRange(Agent centerAgent, float range)
        {
            if (centerAgent?.Team?.ActiveAgents == null)
                return;

            float rangeSq = range * range;
            Vec3 pos = centerAgent.Position;

            EnqueueAgentsToCheer(
                centerAgent.Team.ActiveAgents,
                agent => agent.Position.DistanceSquared(pos) < rangeSq
            );
        }

        public static void EnqueueAgentsToCheer(IEnumerable<Agent> agents, Func<Agent, bool>? filter = null)
        {
            if (agents == null)
                return;

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

            if (settings.LoggingEnabled && enqueuedCount > 0)
            {
                InformationManager.DisplayMessage(new InformationMessage(
                    $"Number of troops queued to cheer: {enqueuedCount}", Colors.Yellow));
            }
        }

        public static void ProcessCheerQueue(float dt)
        {
            if (!_cheeringInProgress)
                return;

            _cheerBatchTimer += dt;
            if (_cheerBatchTimer < CheerBatchInterval)
                return;

            int count = 0;
            while (_cheerQueue.Count > 0 && count < CheerBatchSize)
            {
                var agent = _cheerQueue.Dequeue();
                if (agent != null && agent.IsActive())
                    agent.SetWantsToYell();
                count++;
            }

            _cheerBatchTimer = 0f;

            if (_cheerQueue.Count == 0)
                _cheeringInProgress = false;
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

        private static string? ResolveSound(bool isFriendly)
        {
            if (settings.DisableHeroKnockdownSounds)
                return null;
            return isFriendly ? SoundFriendlyFalls : SoundEnemyFalls;
        }

        public static void DisplayQuickInformationMessageWhenGeneralFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (victimAgent?.Team == null)
                return;

            bool isFriendly = victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly;
            string text = isFriendly ? settings.friendlyGeneralFallenNotification : settings.enemyGeneralFallenNotification;

            MBInformationManager.AddQuickInformation(new TextObject(text), 2000, attackerAgent?.Character, null, ResolveSound(isFriendly));
        }

        public static void DisplayQuickInformationMessageWhenCaptainFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (victimAgent?.Team == null)
                return;

            FormationCategory category = GetFormationCategory(victimAgent.Formation);
            bool isFriendly = victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly;

            string message = (isFriendly, category) switch
            {
                (true, FormationCategory.Infantry)    => settings.friendlyInfantryCaptainFallenNotification,
                (true, FormationCategory.Archers)     => settings.friendlyRangedCaptainFallenNotification,
                (true, FormationCategory.Cavalry)     => settings.friendlyCavalryCaptainFallenNotification,
                (true, FormationCategory.HorseArchers) => settings.friendlyHorseArchersCaptainFallenNotification,
                (true, _)                             => "Your captain has fallen! Give up!",
                (false, FormationCategory.Infantry)   => settings.enemyInfantryCaptainFallenNotification,
                (false, FormationCategory.Archers)    => settings.enemyRangedCaptainFallenNotification,
                (false, FormationCategory.Cavalry)    => settings.enemyCavalryCaptainFallenNotification,
                (false, FormationCategory.HorseArchers) => settings.enemyHorseArchersCaptainFallenNotification,
                (false, _)                            => "Enemy's captain has fallen! Good fight!"
            };

            MBInformationManager.AddQuickInformation(new TextObject(message), 2000, attackerAgent?.Character, null, ResolveSound(isFriendly));
        }

        public static void DisplayQuickInformationMessageWhenUnassignedHeroFalls(Agent attackerAgent, Agent victimAgent)
        {
            if (settings.HideUnassignedHeroNotifications)
                return;

            if (victimAgent?.Team == null)
                return;

            bool isFriendly = victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly;
            string text = isFriendly ? settings.friendlyUnassignedHeroFallenNotification : settings.enemyUnassignedHeroFallenNotification;

            MBInformationManager.AddQuickInformation(new TextObject(text), 2000, attackerAgent?.Character, null, ResolveSound(isFriendly));
        }
    }
}