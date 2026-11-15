using KnockedDownHeroesInfluencesTroops.Core;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Base.Global;

namespace KnockedDownHeroesInfluencesTroops.Data;

// Used only when MCM is loaded. The only file that may use MCM types.
// Kept exception: property names (incl. the lowercase notification texts) are MCM save keys of the released mod.
internal class MCMSettings : AttributeGlobalSettings<MCMSettings>
{
    // Released Id: changing it resets every player's saved settings.
    private const string SettingsId = "KnockedDownHeroesInfluencesTroopsSettings";
    private const string SaveFormat = "json2";
    private const string WholeNumberFormat = "0";
    private const int UnlimitedTextLength = -1;
    private const string NoTextFilter = "";

    public override string Id => SettingsId;

    public override string DisplayName => Texts.ModName.ToString();

    public override string FolderName => ModInfo.ModuleId;

    public override string FormatType => SaveFormat;

    // Main settings

    [SettingPropertyBool(Texts.Ids.EnableThisModification, Order = 0, RequireRestart = false, HintText = Texts.Ids.EnableThisModificationHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMain, GroupOrder = 0)]
    public bool EnableThisModification { get; set; } = ModConfiguration.EnableThisModificationDefault;

    [SettingPropertyBool(Texts.Ids.ShowOnScreenNotifications, Order = 1, RequireRestart = false, HintText = Texts.Ids.ShowOnScreenNotificationsHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMain, GroupOrder = 0)]
    public bool ShowOnScreenNotifications { get; set; } = ModConfiguration.ShowOnScreenNotificationsDefault;

    [SettingPropertyBool(Texts.Ids.HideUnassignedHeroNotifications, Order = 2, RequireRestart = false, HintText = Texts.Ids.HideUnassignedHeroNotificationsHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMain, GroupOrder = 0)]
    public bool HideUnassignedHeroNotifications { get; set; } = ModConfiguration.HideUnassignedHeroNotificationsDefault;

    [SettingPropertyBool(Texts.Ids.DisableKnockdownSounds, Order = 3, RequireRestart = false, HintText = Texts.Ids.DisableKnockdownSoundsHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMain, GroupOrder = 0)]
    public bool DisableKnockdownSounds { get; set; } = ModConfiguration.DisableKnockdownSoundsDefault;

    [SettingPropertyBool(Texts.Ids.PlayHeroFallReactionVoiceLines, Order = 4, RequireRestart = false, HintText = Texts.Ids.PlayHeroFallReactionVoiceLinesHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMain, GroupOrder = 0)]
    public bool PlayHeroFallReactionVoiceLines { get; set; } = ModConfiguration.PlayHeroFallReactionVoiceLinesDefault;

    // Morale penalties when heroes are knocked down

    [SettingPropertyInteger(Texts.Ids.PenaltyTroopKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 0, RequireRestart = false, HintText = Texts.Ids.PenaltyTroopKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenTroopKillsUnassignedHero { get; set; } = ModConfiguration.MoraleChangeWhenTroopKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyTroopKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 1, RequireRestart = false, HintText = Texts.Ids.PenaltyTroopKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenTroopKillsCaptainHero { get; set; } = ModConfiguration.MoraleChangeWhenTroopKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyTroopKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 2, RequireRestart = false, HintText = Texts.Ids.PenaltyTroopKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenTroopKillsGeneralHero { get; set; } = ModConfiguration.MoraleChangeWhenTroopKillsGeneralHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyUnassignedHeroKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 3, RequireRestart = false, HintText = Texts.Ids.PenaltyUnassignedHeroKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenUnassignedHeroKillsUnassignedHero { get; set; } = ModConfiguration.MoraleChangeWhenUnassignedHeroKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyUnassignedHeroKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 4, RequireRestart = false, HintText = Texts.Ids.PenaltyUnassignedHeroKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenUnassignedHeroKillsCaptainHero { get; set; } = ModConfiguration.MoraleChangeWhenUnassignedHeroKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyUnassignedHeroKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 5, RequireRestart = false, HintText = Texts.Ids.PenaltyUnassignedHeroKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenUnassignedHeroKillsGeneralHero { get; set; } = ModConfiguration.MoraleChangeWhenUnassignedHeroKillsGeneralHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyCaptainHeroKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 6, RequireRestart = false, HintText = Texts.Ids.PenaltyCaptainHeroKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenCaptainHeroKillsUnassignedHero { get; set; } = ModConfiguration.MoraleChangeWhenCaptainHeroKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyCaptainHeroKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 7, RequireRestart = false, HintText = Texts.Ids.PenaltyCaptainHeroKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenCaptainHeroKillsCaptainHero { get; set; } = ModConfiguration.MoraleChangeWhenCaptainHeroKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyCaptainHeroKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 8, RequireRestart = false, HintText = Texts.Ids.PenaltyCaptainHeroKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenCaptainHeroKillsGeneralHero { get; set; } = ModConfiguration.MoraleChangeWhenCaptainHeroKillsGeneralHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyGeneralHeroKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 9, RequireRestart = false, HintText = Texts.Ids.PenaltyGeneralHeroKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenGeneralHeroKillsUnassignedHero { get; set; } = ModConfiguration.MoraleChangeWhenGeneralHeroKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyGeneralHeroKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 10, RequireRestart = false, HintText = Texts.Ids.PenaltyGeneralHeroKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenGeneralHeroKillsCaptainHero { get; set; } = ModConfiguration.MoraleChangeWhenGeneralHeroKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.PenaltyGeneralHeroKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 11, RequireRestart = false, HintText = Texts.Ids.PenaltyGeneralHeroKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoralePenalties, GroupOrder = 1)]
    public int MoraleChangeWhenGeneralHeroKillsGeneralHero { get; set; } = ModConfiguration.MoraleChangeWhenGeneralHeroKillsGeneralHeroDefault;

    // Morale gains when heroes are knocked down

    [SettingPropertyInteger(Texts.Ids.GainTroopKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 0, RequireRestart = false, HintText = Texts.Ids.GainTroopKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenTroopKillsUnassignedHero { get; set; } = ModConfiguration.MoraleGainWhenTroopKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainTroopKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 1, RequireRestart = false, HintText = Texts.Ids.GainTroopKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenTroopKillsCaptainHero { get; set; } = ModConfiguration.MoraleGainWhenTroopKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainTroopKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 2, RequireRestart = false, HintText = Texts.Ids.GainTroopKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenTroopKillsGeneralHero { get; set; } = ModConfiguration.MoraleGainWhenTroopKillsGeneralHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainUnassignedHeroKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 3, RequireRestart = false, HintText = Texts.Ids.GainUnassignedHeroKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenUnassignedHeroKillsUnassignedHero { get; set; } = ModConfiguration.MoraleGainWhenUnassignedHeroKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainUnassignedHeroKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 4, RequireRestart = false, HintText = Texts.Ids.GainUnassignedHeroKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenUnassignedHeroKillsCaptainHero { get; set; } = ModConfiguration.MoraleGainWhenUnassignedHeroKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainUnassignedHeroKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 5, RequireRestart = false, HintText = Texts.Ids.GainUnassignedHeroKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenUnassignedHeroKillsGeneralHero { get; set; } = ModConfiguration.MoraleGainWhenUnassignedHeroKillsGeneralHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainCaptainHeroKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 6, RequireRestart = false, HintText = Texts.Ids.GainCaptainHeroKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenCaptainHeroKillsUnassignedHero { get; set; } = ModConfiguration.MoraleGainWhenCaptainHeroKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainCaptainHeroKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 7, RequireRestart = false, HintText = Texts.Ids.GainCaptainHeroKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenCaptainHeroKillsCaptainHero { get; set; } = ModConfiguration.MoraleGainWhenCaptainHeroKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainCaptainHeroKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 8, RequireRestart = false, HintText = Texts.Ids.GainCaptainHeroKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenCaptainHeroKillsGeneralHero { get; set; } = ModConfiguration.MoraleGainWhenCaptainHeroKillsGeneralHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainGeneralHeroKillsUnassignedHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 9, RequireRestart = false, HintText = Texts.Ids.GainGeneralHeroKillsUnassignedHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenGeneralHeroKillsUnassignedHero { get; set; } = ModConfiguration.MoraleGainWhenGeneralHeroKillsUnassignedHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainGeneralHeroKillsCaptainHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 10, RequireRestart = false, HintText = Texts.Ids.GainGeneralHeroKillsCaptainHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenGeneralHeroKillsCaptainHero { get; set; } = ModConfiguration.MoraleGainWhenGeneralHeroKillsCaptainHeroDefault;

    [SettingPropertyInteger(Texts.Ids.GainGeneralHeroKillsGeneralHero, ModConfiguration.MoraleChangeMin, ModConfiguration.MoraleChangeMax, WholeNumberFormat, Order = 11, RequireRestart = false, HintText = Texts.Ids.GainGeneralHeroKillsGeneralHeroHint)]
    [SettingPropertyGroup(Texts.Ids.GroupMoraleGains, GroupOrder = 2)]
    public int MoraleGainWhenGeneralHeroKillsGeneralHero { get; set; } = ModConfiguration.MoraleGainWhenGeneralHeroKillsGeneralHeroDefault;

    // On-screen notifications texts

    [SettingPropertyText(Texts.Ids.FriendlyGeneralFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 0, RequireRestart = false, HintText = Texts.Ids.FriendlyGeneralFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string friendlyGeneralFallenNotification { get; set; } = ModConfiguration.FriendlyGeneralFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.EnemyGeneralFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 1, RequireRestart = false, HintText = Texts.Ids.EnemyGeneralFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string enemyGeneralFallenNotification { get; set; } = ModConfiguration.EnemyGeneralFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.FriendlyInfantryCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 2, RequireRestart = false, HintText = Texts.Ids.FriendlyInfantryCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string friendlyInfantryCaptainFallenNotification { get; set; } = ModConfiguration.FriendlyInfantryCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.FriendlyRangedCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 3, RequireRestart = false, HintText = Texts.Ids.FriendlyRangedCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string friendlyRangedCaptainFallenNotification { get; set; } = ModConfiguration.FriendlyRangedCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.FriendlyCavalryCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 4, RequireRestart = false, HintText = Texts.Ids.FriendlyCavalryCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string friendlyCavalryCaptainFallenNotification { get; set; } = ModConfiguration.FriendlyCavalryCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.FriendlyHorseArchersCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 5, RequireRestart = false, HintText = Texts.Ids.FriendlyHorseArchersCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string friendlyHorseArchersCaptainFallenNotification { get; set; } = ModConfiguration.FriendlyHorseArchersCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.EnemyInfantryCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 6, RequireRestart = false, HintText = Texts.Ids.EnemyInfantryCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string enemyInfantryCaptainFallenNotification { get; set; } = ModConfiguration.EnemyInfantryCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.EnemyRangedCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 7, RequireRestart = false, HintText = Texts.Ids.EnemyRangedCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string enemyRangedCaptainFallenNotification { get; set; } = ModConfiguration.EnemyRangedCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.EnemyCavalryCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 8, RequireRestart = false, HintText = Texts.Ids.EnemyCavalryCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string enemyCavalryCaptainFallenNotification { get; set; } = ModConfiguration.EnemyCavalryCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.EnemyHorseArchersCaptainFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 9, RequireRestart = false, HintText = Texts.Ids.EnemyHorseArchersCaptainFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string enemyHorseArchersCaptainFallenNotification { get; set; } = ModConfiguration.EnemyHorseArchersCaptainFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.FriendlyUnassignedHeroFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 10, RequireRestart = false, HintText = Texts.Ids.FriendlyUnassignedHeroFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string friendlyUnassignedHeroFallenNotification { get; set; } = ModConfiguration.FriendlyUnassignedHeroFallenNotificationDefault;

    [SettingPropertyText(Texts.Ids.EnemyUnassignedHeroFallenLabel, UnlimitedTextLength, true, NoTextFilter, Order = 11, RequireRestart = false, HintText = Texts.Ids.EnemyUnassignedHeroFallenLabelHint)]
    [SettingPropertyGroup(Texts.Ids.GroupNotificationTexts, GroupOrder = 3)]
    public string enemyUnassignedHeroFallenNotification { get; set; } = ModConfiguration.EnemyUnassignedHeroFallenNotificationDefault;

    // "Technical settings" is always the last group and "Logging for debugging" its last entry.

    [SettingPropertyInteger(Texts.Ids.UpdateIntervalInSeconds, ModConfiguration.UpdateIntervalInSecondsMin, ModConfiguration.UpdateIntervalInSecondsMax, WholeNumberFormat, Order = 1, RequireRestart = false, HintText = Texts.Ids.UpdateIntervalInSecondsHint)]
    [SettingPropertyGroup(Texts.Ids.GroupTechnical, GroupOrder = 4)]
    public int UpdateIntervalInSeconds { get; set; } = ModConfiguration.UpdateIntervalInSecondsDefault;

    [SettingPropertyBool(Texts.Ids.LoggingEnabled, Order = 2, RequireRestart = false, HintText = Texts.Ids.LoggingEnabledHint)]
    [SettingPropertyGroup(Texts.Ids.GroupTechnical, GroupOrder = 4)]
    public bool LoggingEnabled { get; set; } = ModConfiguration.LoggingEnabledDefault;
}
