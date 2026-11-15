using System.Collections.Generic;
using KnockedDownHeroesInfluencesTroops.Core;
using KnockedDownHeroesInfluencesTroops.Data;
using KnockedDownHeroesInfluencesTroops.Diagnostics;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops.Missions;

internal static class MissionUtilities
{
    private static readonly Queue<Agent> CheerQueue = new();
    private static float _cheerBatchTimer;
    private static bool _cheeringInProgress;

    private const string GeneralFallReactionSoundMale = "kdhit_general_fall_reaction_male";
    private const string GeneralFallReactionSoundFemale = "kdhit_general_fall_reaction_female";
    private const string CaptainFallReactionSoundMale = "kdhit_captain_fall_reaction_male";
    private const string CaptainFallReactionSoundFemale = "kdhit_captain_fall_reaction_female";

    private static readonly Dictionary<string, int> FallReactionSoundIdCache = new();

    private const string SoundFriendlyFalls = "event:/ui/notification/death";
    private const string SoundEnemyFalls = "event:/ui/notification/levelup";

    private const string FriendlyLogLabel = "Friendly";
    private const string EnemyLogLabel = "Enemy";
    private const string UnknownAgentName = "Unknown";

    private static string ResolveSound(bool isFriendly) => ModSettings.DisableKnockdownSounds ? string.Empty : isFriendly ? SoundFriendlyFalls : SoundEnemyFalls;

    internal static void PlayHeroFallReactionVoiceLine(Agent victimAgent, bool victimIsGeneral)
    {
        if (!ModSettings.PlayHeroFallReactionVoiceLines || Mission.Current == null)
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

    internal static void UpdateMoraleForNearbyAgents(Team team, Agent referenceAgent, float range, float moraleChange)
    {
        int troopCount = 0;
        float rangeSquared = range * range;
        foreach (Agent agent in team.ActiveAgents)
        {
            if (agent.Position.DistanceSquared(referenceAgent.Position) < rangeSquared)
            {
                agent.ChangeMorale(moraleChange);
                troopCount++;
            }
        }

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"Morale {moraleChange:+0;-0;0} for troops within {range} m: {troopCount} troops.");
    }

    internal static void UpdateTeamMorale(Team affectedTeam, float moraleChange)
    {
        int troopCount = 0;

        foreach (Agent agent in affectedTeam.ActiveAgents)
        {
            agent.ChangeMorale(moraleChange);
            troopCount++;
        }

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"Morale {moraleChange:+0;-0;0} for the whole team: {troopCount} troops.");
    }

    internal static void UpdateFormationMorale(Dictionary<Agent, List<Agent>> troopsOfFormationCaptains, Agent formationCaptain, float moraleChange)
    {
        int troopCount = 0;
        List<Agent> troops = troopsOfFormationCaptains[formationCaptain];

        for (int i = 0; i < troops.Count; i++)
        {
            Agent agent = troops[i];
            if (!agent.IsActive())
                continue;
            agent.ChangeMorale(moraleChange);
            troopCount++;
        }

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"Morale {moraleChange:+0;-0;0} for the captain's formation: {troopCount} troops.");
    }

    internal static void SetWantsToYellForTeam(Team team)
    {
        int enqueuedCount = 0;
        foreach (Agent agent in team.ActiveAgents)
        {
            if (TryEnqueueCheer(agent))
                enqueuedCount++;
        }

        FinishEnqueue(enqueuedCount);
    }

    internal static void SetWantsToYellForFormation(List<Agent> troopsInFormation)
    {
        int enqueuedCount = 0;
        for (int i = 0; i < troopsInFormation.Count; i++)
        {
            if (TryEnqueueCheer(troopsInFormation[i]))
                enqueuedCount++;
        }

        FinishEnqueue(enqueuedCount);
    }

    internal static void SetWantsToYellInRange(Agent attackerAgent, float range)
    {
        if (attackerAgent.Team?.ActiveAgents == null)
            return;

        float rangeSquared = range * range;
        Vec3 origin = attackerAgent.Position;
        int enqueuedCount = 0;
        foreach (Agent agent in attackerAgent.Team.ActiveAgents)
        {
            if (agent != null && agent.Position.DistanceSquared(origin) < rangeSquared && TryEnqueueCheer(agent))
                enqueuedCount++;
        }

        FinishEnqueue(enqueuedCount);
    }

    private static bool TryEnqueueCheer(Agent? agent)
    {
        if (agent == null)
            return false;
        CheerQueue.Enqueue(agent);
        return true;
    }

    private static void FinishEnqueue(int enqueuedCount)
    {
        if (enqueuedCount <= 0)
            return;

        _cheeringInProgress = true;

        if (ModSettings.LoggingEnabled)
            KdhitLog.Debug($"Troops queued to cheer: {enqueuedCount}.");
    }

    internal static void ProcessCheerQueue(float dt)
    {
        if (!_cheeringInProgress)
            return;

        _cheerBatchTimer += dt;
        if (_cheerBatchTimer < Balance.CheerBatchIntervalSeconds)
            return;

        int count = 0;
        while (CheerQueue.Count > 0 && count < Balance.CheerBatchSize)
        {
            Agent agent = CheerQueue.Dequeue();
            if (!agent.IsActive())
                continue;

            agent.SetWantsToYell();
            count++;
        }

        _cheerBatchTimer = 0f;

        if (CheerQueue.Count == 0)
            _cheeringInProgress = false;
    }

    internal static bool IsAgentGeneral(Agent agent) => agent.Team?.GeneralAgent == agent;

    internal static bool IsAgentCaptain(Agent agent, List<Agent> friendlyInfantryCaptains, List<Agent> friendlyArchersCaptains, List<Agent> friendlyCavalryCaptains, List<Agent> friendlyHorseArchersCaptains, List<Agent> enemyInfantryCaptains, List<Agent> enemyArchersCaptains, List<Agent> enemyCavalryCaptains, List<Agent> enemyHorseArchersCaptains)
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

    internal static FormationKind GetFormationKind(Formation? formation)
    {
        if (formation == null)
            return FormationKind.Unknown;
        if (formation.QuerySystem.IsInfantryFormation)
            return FormationKind.Infantry;
        if (formation.QuerySystem.IsRangedFormation)
            return FormationKind.Archers;
        if (formation.QuerySystem.IsCavalryFormation)
            return FormationKind.Cavalry;
        if (formation.QuerySystem.IsRangedCavalryFormation)
            return FormationKind.HorseArchers;
        return FormationKind.Unknown;
    }

    internal static string SideLogLabel(bool isPlayerAlly) => isPlayerAlly ? FriendlyLogLabel : EnemyLogLabel;

    // Debug only: callers check ModSettings.LoggingEnabled first.
    internal static void LogKnockdown(Agent? attackerAgent, Agent? victimAgent)
    {
        if (attackerAgent == null || victimAgent == null || attackerAgent.Team == null || victimAgent.Team == null)
            return;

        string affectorName = attackerAgent.Name ?? UnknownAgentName;
        string affectedName = victimAgent.Name ?? UnknownAgentName;
        KdhitLog.Debug($"{SideLogLabel(attackerAgent.Team.IsPlayerAlly)} {affectorName} knocked down {affectedName}.");
    }

    internal static void DisplayQuickInformationMessageWhenGeneralFalls(Agent attackerAgent, Agent victimAgent)
    {
        if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
            ShowQuickInformation(NotificationText(ModSettings.FriendlyGeneralFallenNotification, ModConfiguration.FriendlyGeneralFallenNotificationDefault, Texts.FriendlyGeneralFallen), attackerAgent, true);
        if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
            ShowQuickInformation(NotificationText(ModSettings.EnemyGeneralFallenNotification, ModConfiguration.EnemyGeneralFallenNotificationDefault, Texts.EnemyGeneralFallen), attackerAgent, false);
    }

    internal static void DisplayQuickInformationMessageWhenCaptainFalls(Agent attackerAgent, Agent victimAgent)
    {
        FormationKind formationKind = GetFormationKind(victimAgent.Formation);

        if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
        {
            TextObject message = formationKind switch
            {
                FormationKind.Infantry => NotificationText(ModSettings.FriendlyInfantryCaptainFallenNotification, ModConfiguration.FriendlyInfantryCaptainFallenNotificationDefault, Texts.FriendlyInfantryCaptainFallen),
                FormationKind.Archers => NotificationText(ModSettings.FriendlyRangedCaptainFallenNotification, ModConfiguration.FriendlyRangedCaptainFallenNotificationDefault, Texts.FriendlyRangedCaptainFallen),
                FormationKind.Cavalry => NotificationText(ModSettings.FriendlyCavalryCaptainFallenNotification, ModConfiguration.FriendlyCavalryCaptainFallenNotificationDefault, Texts.FriendlyCavalryCaptainFallen),
                FormationKind.HorseArchers => NotificationText(ModSettings.FriendlyHorseArchersCaptainFallenNotification, ModConfiguration.FriendlyHorseArchersCaptainFallenNotificationDefault, Texts.FriendlyHorseArchersCaptainFallen),
                _ => Texts.FriendlyCaptainFallen
            };
            ShowQuickInformation(message, attackerAgent, true);
        }
        else if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
        {
            TextObject message = formationKind switch
            {
                FormationKind.Infantry => NotificationText(ModSettings.EnemyInfantryCaptainFallenNotification, ModConfiguration.EnemyInfantryCaptainFallenNotificationDefault, Texts.EnemyInfantryCaptainFallen),
                FormationKind.Archers => NotificationText(ModSettings.EnemyRangedCaptainFallenNotification, ModConfiguration.EnemyRangedCaptainFallenNotificationDefault, Texts.EnemyRangedCaptainFallen),
                FormationKind.Cavalry => NotificationText(ModSettings.EnemyCavalryCaptainFallenNotification, ModConfiguration.EnemyCavalryCaptainFallenNotificationDefault, Texts.EnemyCavalryCaptainFallen),
                FormationKind.HorseArchers => NotificationText(ModSettings.EnemyHorseArchersCaptainFallenNotification, ModConfiguration.EnemyHorseArchersCaptainFallenNotificationDefault, Texts.EnemyHorseArchersCaptainFallen),
                _ => Texts.EnemyCaptainFallen
            };
            ShowQuickInformation(message, attackerAgent, false);
        }
    }

    internal static void DisplayQuickInformationMessageWhenUnassignedHeroFalls(Agent attackerAgent, Agent victimAgent)
    {
        if (ModSettings.HideUnassignedHeroNotifications)
            return;

        if (victimAgent.Team.IsPlayerTeam || victimAgent.Team.IsPlayerAlly)
            ShowQuickInformation(NotificationText(ModSettings.FriendlyUnassignedHeroFallenNotification, ModConfiguration.FriendlyUnassignedHeroFallenNotificationDefault, Texts.FriendlyUnassignedHeroFallen), attackerAgent, true);
        if (!victimAgent.Team.IsPlayerTeam && !victimAgent.Team.IsPlayerAlly)
            ShowQuickInformation(NotificationText(ModSettings.EnemyUnassignedHeroFallenNotification, ModConfiguration.EnemyUnassignedHeroFallenNotificationDefault, Texts.EnemyUnassignedHeroFallen), attackerAgent, false);
    }

    // A text still equal to its English default is shown translated; a text the player changed is shown as typed.
    private static TextObject NotificationText(string value, string englishDefault, TextObject translated) => value == englishDefault ? translated : new TextObject(value);

    private static void ShowQuickInformation(TextObject message, Agent attackerAgent, bool isFriendly) =>
        MBInformationManager.AddQuickInformation(message, Balance.NotificationDurationMs, attackerAgent.Character, null, ResolveSound(isFriendly));

    internal static void ResetCheerState()
    {
        CheerQueue.Clear();
        _cheerBatchTimer = 0f;
        _cheeringInProgress = false;
    }
}
