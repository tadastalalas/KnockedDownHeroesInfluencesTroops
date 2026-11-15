namespace KnockedDownHeroesInfluencesTroops.Core;

// Gameplay numbers (not player settings). Values = the mod's released behavior.
internal static class Balance
{
    // Radius in metres around the fallen hero (losses) or the attacker (gains, cheering), by the fallen hero's role.
    internal const float UnassignedHeroFallRange = 10f;
    internal const float CaptainFallRange = 20f;
    internal const float GeneralFallRange = 30f;

    // Added to UnassignedHeroFallRange when a formation captain knocks down an unassigned hero.
    internal const float CaptainAttackerExtraRange = 10f;

    // Added to the fall range for cheering (and the captain-fall gain) when an unassigned hero is the attacker.
    internal const float UnassignedHeroAttackerExtraRange = 5f;

    // Cheering is spread over frames: this many agents yell per batch, one batch every CheerBatchIntervalSeconds.
    internal const int CheerBatchSize = 10;
    internal const float CheerBatchIntervalSeconds = 0.1f;

    // Extra time an on-screen notification stays visible.
    internal const int NotificationDurationMs = 2000;
}
