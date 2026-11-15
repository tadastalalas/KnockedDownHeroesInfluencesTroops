using System;

namespace KnockedDownHeroesInfluencesTroops.Data;

// Shape of ModConfiguration.xml and the only place where defaults and ranges live (MCMSettings uses them too).
public class ModConfiguration
{
    public const bool EnableThisModificationDefault = true;
    public const bool ShowOnScreenNotificationsDefault = true;
    public const bool HideUnassignedHeroNotificationsDefault = false;
    public const bool DisableKnockdownSoundsDefault = false;
    public const bool PlayHeroFallReactionVoiceLinesDefault = true;

    // Every morale penalty and gain uses this range.
    public const int MoraleChangeMin = 0;
    public const int MoraleChangeMax = 100;

    public const int MoraleChangeWhenTroopKillsUnassignedHeroDefault = 2;
    public const int MoraleChangeWhenTroopKillsCaptainHeroDefault = 3;
    public const int MoraleChangeWhenTroopKillsGeneralHeroDefault = 4;
    public const int MoraleChangeWhenUnassignedHeroKillsUnassignedHeroDefault = 6;
    public const int MoraleChangeWhenUnassignedHeroKillsCaptainHeroDefault = 7;
    public const int MoraleChangeWhenUnassignedHeroKillsGeneralHeroDefault = 8;
    public const int MoraleChangeWhenCaptainHeroKillsUnassignedHeroDefault = 10;
    public const int MoraleChangeWhenCaptainHeroKillsCaptainHeroDefault = 11;
    public const int MoraleChangeWhenCaptainHeroKillsGeneralHeroDefault = 12;
    public const int MoraleChangeWhenGeneralHeroKillsUnassignedHeroDefault = 14;
    public const int MoraleChangeWhenGeneralHeroKillsCaptainHeroDefault = 15;
    public const int MoraleChangeWhenGeneralHeroKillsGeneralHeroDefault = 16;

    public const int MoraleGainWhenTroopKillsUnassignedHeroDefault = 2;
    public const int MoraleGainWhenTroopKillsCaptainHeroDefault = 3;
    public const int MoraleGainWhenTroopKillsGeneralHeroDefault = 4;
    public const int MoraleGainWhenUnassignedHeroKillsUnassignedHeroDefault = 6;
    public const int MoraleGainWhenUnassignedHeroKillsCaptainHeroDefault = 7;
    public const int MoraleGainWhenUnassignedHeroKillsGeneralHeroDefault = 8;
    public const int MoraleGainWhenCaptainHeroKillsUnassignedHeroDefault = 10;
    public const int MoraleGainWhenCaptainHeroKillsCaptainHeroDefault = 11;
    public const int MoraleGainWhenCaptainHeroKillsGeneralHeroDefault = 12;
    public const int MoraleGainWhenGeneralHeroKillsUnassignedHeroDefault = 14;
    public const int MoraleGainWhenGeneralHeroKillsCaptainHeroDefault = 15;
    public const int MoraleGainWhenGeneralHeroKillsGeneralHeroDefault = 16;

    // English defaults of the notification texts. Texts.Ids builds the translated ids from them;
    // a value still equal to its default is shown translated, any other value as typed.
    public const string FriendlyGeneralFallenNotificationDefault = "Your general has fallen! Nobody will save you!";
    public const string EnemyGeneralFallenNotificationDefault = "Enemy's general has fallen! Victory is ours!";
    public const string FriendlyInfantryCaptainFallenNotificationDefault = "Your infantry captain has fallen! Give up!";
    public const string FriendlyRangedCaptainFallenNotificationDefault = "Your ranged captain has fallen! Give up!";
    public const string FriendlyCavalryCaptainFallenNotificationDefault = "Your cavalry captain has fallen! Give up!";
    public const string FriendlyHorseArchersCaptainFallenNotificationDefault = "Your horse archers captain has fallen! Give up!";
    public const string EnemyInfantryCaptainFallenNotificationDefault = "Enemy's infantry captain has fallen! Good fight!";
    public const string EnemyRangedCaptainFallenNotificationDefault = "Enemy's archers captain has fallen! Good fight!";
    public const string EnemyCavalryCaptainFallenNotificationDefault = "Enemy's cavalry captain has fallen! Good fight!";
    public const string EnemyHorseArchersCaptainFallenNotificationDefault = "Enemy's horse archers captain has fallen! Good fight!";
    public const string FriendlyUnassignedHeroFallenNotificationDefault = "One of your heroes has fallen!";
    public const string EnemyUnassignedHeroFallenNotificationDefault = "One of their heroes has fallen!";

    public const int UpdateIntervalInSecondsDefault = 30;
    public const int UpdateIntervalInSecondsMin = 1;
    public const int UpdateIntervalInSecondsMax = 100;
    public const bool LoggingEnabledDefault = false;

    public MainSettingsSection MainSettings { get; set; } = new();

    public MoralePenaltiesSection MoralePenalties { get; set; } = new();

    public MoraleGainsSection MoraleGains { get; set; } = new();

    public NotificationTextsSection NotificationTexts { get; set; } = new();

    // Always the last section.
    public TechnicalSettingsSection TechnicalSettings { get; set; } = new();

    internal void Clamp()
    {
        MoralePenaltiesSection p = MoralePenalties;
        p.MoraleChangeWhenTroopKillsUnassignedHero = ClampMorale(p.MoraleChangeWhenTroopKillsUnassignedHero);
        p.MoraleChangeWhenTroopKillsCaptainHero = ClampMorale(p.MoraleChangeWhenTroopKillsCaptainHero);
        p.MoraleChangeWhenTroopKillsGeneralHero = ClampMorale(p.MoraleChangeWhenTroopKillsGeneralHero);
        p.MoraleChangeWhenUnassignedHeroKillsUnassignedHero = ClampMorale(p.MoraleChangeWhenUnassignedHeroKillsUnassignedHero);
        p.MoraleChangeWhenUnassignedHeroKillsCaptainHero = ClampMorale(p.MoraleChangeWhenUnassignedHeroKillsCaptainHero);
        p.MoraleChangeWhenUnassignedHeroKillsGeneralHero = ClampMorale(p.MoraleChangeWhenUnassignedHeroKillsGeneralHero);
        p.MoraleChangeWhenCaptainHeroKillsUnassignedHero = ClampMorale(p.MoraleChangeWhenCaptainHeroKillsUnassignedHero);
        p.MoraleChangeWhenCaptainHeroKillsCaptainHero = ClampMorale(p.MoraleChangeWhenCaptainHeroKillsCaptainHero);
        p.MoraleChangeWhenCaptainHeroKillsGeneralHero = ClampMorale(p.MoraleChangeWhenCaptainHeroKillsGeneralHero);
        p.MoraleChangeWhenGeneralHeroKillsUnassignedHero = ClampMorale(p.MoraleChangeWhenGeneralHeroKillsUnassignedHero);
        p.MoraleChangeWhenGeneralHeroKillsCaptainHero = ClampMorale(p.MoraleChangeWhenGeneralHeroKillsCaptainHero);
        p.MoraleChangeWhenGeneralHeroKillsGeneralHero = ClampMorale(p.MoraleChangeWhenGeneralHeroKillsGeneralHero);

        MoraleGainsSection g = MoraleGains;
        g.MoraleGainWhenTroopKillsUnassignedHero = ClampMorale(g.MoraleGainWhenTroopKillsUnassignedHero);
        g.MoraleGainWhenTroopKillsCaptainHero = ClampMorale(g.MoraleGainWhenTroopKillsCaptainHero);
        g.MoraleGainWhenTroopKillsGeneralHero = ClampMorale(g.MoraleGainWhenTroopKillsGeneralHero);
        g.MoraleGainWhenUnassignedHeroKillsUnassignedHero = ClampMorale(g.MoraleGainWhenUnassignedHeroKillsUnassignedHero);
        g.MoraleGainWhenUnassignedHeroKillsCaptainHero = ClampMorale(g.MoraleGainWhenUnassignedHeroKillsCaptainHero);
        g.MoraleGainWhenUnassignedHeroKillsGeneralHero = ClampMorale(g.MoraleGainWhenUnassignedHeroKillsGeneralHero);
        g.MoraleGainWhenCaptainHeroKillsUnassignedHero = ClampMorale(g.MoraleGainWhenCaptainHeroKillsUnassignedHero);
        g.MoraleGainWhenCaptainHeroKillsCaptainHero = ClampMorale(g.MoraleGainWhenCaptainHeroKillsCaptainHero);
        g.MoraleGainWhenCaptainHeroKillsGeneralHero = ClampMorale(g.MoraleGainWhenCaptainHeroKillsGeneralHero);
        g.MoraleGainWhenGeneralHeroKillsUnassignedHero = ClampMorale(g.MoraleGainWhenGeneralHeroKillsUnassignedHero);
        g.MoraleGainWhenGeneralHeroKillsCaptainHero = ClampMorale(g.MoraleGainWhenGeneralHeroKillsCaptainHero);
        g.MoraleGainWhenGeneralHeroKillsGeneralHero = ClampMorale(g.MoraleGainWhenGeneralHeroKillsGeneralHero);

        // xsi:nil or similar can leave a text null; an empty text stays empty (as in MCM).
        NotificationTextsSection n = NotificationTexts;
        n.FriendlyGeneralFallenNotification ??= FriendlyGeneralFallenNotificationDefault;
        n.EnemyGeneralFallenNotification ??= EnemyGeneralFallenNotificationDefault;
        n.FriendlyInfantryCaptainFallenNotification ??= FriendlyInfantryCaptainFallenNotificationDefault;
        n.FriendlyRangedCaptainFallenNotification ??= FriendlyRangedCaptainFallenNotificationDefault;
        n.FriendlyCavalryCaptainFallenNotification ??= FriendlyCavalryCaptainFallenNotificationDefault;
        n.FriendlyHorseArchersCaptainFallenNotification ??= FriendlyHorseArchersCaptainFallenNotificationDefault;
        n.EnemyInfantryCaptainFallenNotification ??= EnemyInfantryCaptainFallenNotificationDefault;
        n.EnemyRangedCaptainFallenNotification ??= EnemyRangedCaptainFallenNotificationDefault;
        n.EnemyCavalryCaptainFallenNotification ??= EnemyCavalryCaptainFallenNotificationDefault;
        n.EnemyHorseArchersCaptainFallenNotification ??= EnemyHorseArchersCaptainFallenNotificationDefault;
        n.FriendlyUnassignedHeroFallenNotification ??= FriendlyUnassignedHeroFallenNotificationDefault;
        n.EnemyUnassignedHeroFallenNotification ??= EnemyUnassignedHeroFallenNotificationDefault;

        TechnicalSettings.UpdateIntervalInSeconds = Math.Min(Math.Max(TechnicalSettings.UpdateIntervalInSeconds, UpdateIntervalInSecondsMin), UpdateIntervalInSecondsMax);
    }

    private static int ClampMorale(int value) => Math.Min(Math.Max(value, MoraleChangeMin), MoraleChangeMax);

    public class MainSettingsSection
    {
        public bool EnableThisModification { get; set; } = EnableThisModificationDefault;
        public bool ShowOnScreenNotifications { get; set; } = ShowOnScreenNotificationsDefault;
        public bool HideUnassignedHeroNotifications { get; set; } = HideUnassignedHeroNotificationsDefault;
        public bool DisableKnockdownSounds { get; set; } = DisableKnockdownSoundsDefault;
        public bool PlayHeroFallReactionVoiceLines { get; set; } = PlayHeroFallReactionVoiceLinesDefault;
    }

    public class MoralePenaltiesSection
    {
        public int MoraleChangeWhenTroopKillsUnassignedHero { get; set; } = MoraleChangeWhenTroopKillsUnassignedHeroDefault;
        public int MoraleChangeWhenTroopKillsCaptainHero { get; set; } = MoraleChangeWhenTroopKillsCaptainHeroDefault;
        public int MoraleChangeWhenTroopKillsGeneralHero { get; set; } = MoraleChangeWhenTroopKillsGeneralHeroDefault;
        public int MoraleChangeWhenUnassignedHeroKillsUnassignedHero { get; set; } = MoraleChangeWhenUnassignedHeroKillsUnassignedHeroDefault;
        public int MoraleChangeWhenUnassignedHeroKillsCaptainHero { get; set; } = MoraleChangeWhenUnassignedHeroKillsCaptainHeroDefault;
        public int MoraleChangeWhenUnassignedHeroKillsGeneralHero { get; set; } = MoraleChangeWhenUnassignedHeroKillsGeneralHeroDefault;
        public int MoraleChangeWhenCaptainHeroKillsUnassignedHero { get; set; } = MoraleChangeWhenCaptainHeroKillsUnassignedHeroDefault;
        public int MoraleChangeWhenCaptainHeroKillsCaptainHero { get; set; } = MoraleChangeWhenCaptainHeroKillsCaptainHeroDefault;
        public int MoraleChangeWhenCaptainHeroKillsGeneralHero { get; set; } = MoraleChangeWhenCaptainHeroKillsGeneralHeroDefault;
        public int MoraleChangeWhenGeneralHeroKillsUnassignedHero { get; set; } = MoraleChangeWhenGeneralHeroKillsUnassignedHeroDefault;
        public int MoraleChangeWhenGeneralHeroKillsCaptainHero { get; set; } = MoraleChangeWhenGeneralHeroKillsCaptainHeroDefault;
        public int MoraleChangeWhenGeneralHeroKillsGeneralHero { get; set; } = MoraleChangeWhenGeneralHeroKillsGeneralHeroDefault;
    }

    public class MoraleGainsSection
    {
        public int MoraleGainWhenTroopKillsUnassignedHero { get; set; } = MoraleGainWhenTroopKillsUnassignedHeroDefault;
        public int MoraleGainWhenTroopKillsCaptainHero { get; set; } = MoraleGainWhenTroopKillsCaptainHeroDefault;
        public int MoraleGainWhenTroopKillsGeneralHero { get; set; } = MoraleGainWhenTroopKillsGeneralHeroDefault;
        public int MoraleGainWhenUnassignedHeroKillsUnassignedHero { get; set; } = MoraleGainWhenUnassignedHeroKillsUnassignedHeroDefault;
        public int MoraleGainWhenUnassignedHeroKillsCaptainHero { get; set; } = MoraleGainWhenUnassignedHeroKillsCaptainHeroDefault;
        public int MoraleGainWhenUnassignedHeroKillsGeneralHero { get; set; } = MoraleGainWhenUnassignedHeroKillsGeneralHeroDefault;
        public int MoraleGainWhenCaptainHeroKillsUnassignedHero { get; set; } = MoraleGainWhenCaptainHeroKillsUnassignedHeroDefault;
        public int MoraleGainWhenCaptainHeroKillsCaptainHero { get; set; } = MoraleGainWhenCaptainHeroKillsCaptainHeroDefault;
        public int MoraleGainWhenCaptainHeroKillsGeneralHero { get; set; } = MoraleGainWhenCaptainHeroKillsGeneralHeroDefault;
        public int MoraleGainWhenGeneralHeroKillsUnassignedHero { get; set; } = MoraleGainWhenGeneralHeroKillsUnassignedHeroDefault;
        public int MoraleGainWhenGeneralHeroKillsCaptainHero { get; set; } = MoraleGainWhenGeneralHeroKillsCaptainHeroDefault;
        public int MoraleGainWhenGeneralHeroKillsGeneralHero { get; set; } = MoraleGainWhenGeneralHeroKillsGeneralHeroDefault;
    }

    public class NotificationTextsSection
    {
        public string FriendlyGeneralFallenNotification { get; set; } = FriendlyGeneralFallenNotificationDefault;
        public string EnemyGeneralFallenNotification { get; set; } = EnemyGeneralFallenNotificationDefault;
        public string FriendlyInfantryCaptainFallenNotification { get; set; } = FriendlyInfantryCaptainFallenNotificationDefault;
        public string FriendlyRangedCaptainFallenNotification { get; set; } = FriendlyRangedCaptainFallenNotificationDefault;
        public string FriendlyCavalryCaptainFallenNotification { get; set; } = FriendlyCavalryCaptainFallenNotificationDefault;
        public string FriendlyHorseArchersCaptainFallenNotification { get; set; } = FriendlyHorseArchersCaptainFallenNotificationDefault;
        public string EnemyInfantryCaptainFallenNotification { get; set; } = EnemyInfantryCaptainFallenNotificationDefault;
        public string EnemyRangedCaptainFallenNotification { get; set; } = EnemyRangedCaptainFallenNotificationDefault;
        public string EnemyCavalryCaptainFallenNotification { get; set; } = EnemyCavalryCaptainFallenNotificationDefault;
        public string EnemyHorseArchersCaptainFallenNotification { get; set; } = EnemyHorseArchersCaptainFallenNotificationDefault;
        public string FriendlyUnassignedHeroFallenNotification { get; set; } = FriendlyUnassignedHeroFallenNotificationDefault;
        public string EnemyUnassignedHeroFallenNotification { get; set; } = EnemyUnassignedHeroFallenNotificationDefault;
    }

    public class TechnicalSettingsSection
    {
        public int UpdateIntervalInSeconds { get; set; } = UpdateIntervalInSecondsDefault;

        // Always the last entry.
        public bool LoggingEnabled { get; set; } = LoggingEnabledDefault;
    }
}
