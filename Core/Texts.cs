using KnockedDownHeroesInfluencesTroops.Data;
using TaleWorlds.Localization;

namespace KnockedDownHeroesInfluencesTroops.Core;

internal static class Texts
{
    #region Settings
    public static readonly TextObject ModName = new(Ids.ModName);
    #endregion

    #region Notifications
    public static readonly TextObject FriendlyGeneralFallen = new(Ids.FriendlyGeneralFallen);
    public static readonly TextObject EnemyGeneralFallen = new(Ids.EnemyGeneralFallen);
    public static readonly TextObject FriendlyInfantryCaptainFallen = new(Ids.FriendlyInfantryCaptainFallen);
    public static readonly TextObject FriendlyRangedCaptainFallen = new(Ids.FriendlyRangedCaptainFallen);
    public static readonly TextObject FriendlyCavalryCaptainFallen = new(Ids.FriendlyCavalryCaptainFallen);
    public static readonly TextObject FriendlyHorseArchersCaptainFallen = new(Ids.FriendlyHorseArchersCaptainFallen);
    public static readonly TextObject EnemyInfantryCaptainFallen = new(Ids.EnemyInfantryCaptainFallen);
    public static readonly TextObject EnemyRangedCaptainFallen = new(Ids.EnemyRangedCaptainFallen);
    public static readonly TextObject EnemyCavalryCaptainFallen = new(Ids.EnemyCavalryCaptainFallen);
    public static readonly TextObject EnemyHorseArchersCaptainFallen = new(Ids.EnemyHorseArchersCaptainFallen);
    public static readonly TextObject FriendlyUnassignedHeroFallen = new(Ids.FriendlyUnassignedHeroFallen);
    public static readonly TextObject EnemyUnassignedHeroFallen = new(Ids.EnemyUnassignedHeroFallen);
    public static readonly TextObject FriendlyCaptainFallen = new(Ids.FriendlyCaptainFallen);
    public static readonly TextObject EnemyCaptainFallen = new(Ids.EnemyCaptainFallen);
    #endregion

    internal static class Ids
    {
        #region Settings
        public const string ModName = "{=KDHIT_yz6lxdJ}Knocked Down Heroes Influences Troops";

        public const string GroupMain = "{=KDHIT_AjEK8im}Main settings";
        public const string EnableThisModification = "{=KDHIT_4qcKDe7}Enable This Modification";
        public const string EnableThisModificationHint = "{=KDHIT_ouqvaSg}Toggle this modification. [Default: enabled]";
        public const string ShowOnScreenNotifications = "{=KDHIT_bJQyNOn}Show On-Screen Notifications";
        public const string ShowOnScreenNotificationsHint = "{=KDHIT_2EJdYrV}Toggle on-screen notifications when any hero is knocked down. [Default: enabled]";
        public const string HideUnassignedHeroNotifications = "{=KDHIT_hOGKEE2}Hide Unassigned Hero Notifications";
        public const string HideUnassignedHeroNotificationsHint = "{=KDHIT_KoXnreW}Hide on-screen notifications when unassigned hero is knocked down. [Default: disabled]";
        public const string DisableKnockdownSounds = "{=KDHIT_Z30i9MU}Disable Hero Knockdown Sounds";
        public const string DisableKnockdownSoundsHint = "{=KDHIT_3ol4FO4}When enabled, the UI sound played when a hero is knocked down will be muted. The on-screen notification still appears. [Default: disabled]";
        public const string PlayHeroFallReactionVoiceLines = "{=KDHIT_hFq3Zt9}Play Hero Fall Reaction Voice Lines";
        public const string PlayHeroFallReactionVoiceLinesHint = "{=KDHIT_Bn7pXo2}Toggle voice lines said by your character when a general or formation captain, friendly or enemy, is knocked down in battle. [Default: enabled]";

        public const string GroupMoralePenalties = "{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down";
        public const string PenaltyTroopKillsUnassignedHero = "{=KDHIT_c4srbaQ}When Troop Knocks Down Unassigned Hero";
        public const string PenaltyTroopKillsUnassignedHeroHint = "{=KDHIT_tyxCqZv}The enemy's troops (all enemy's troops within 10m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 2]";
        public const string PenaltyTroopKillsCaptainHero = "{=KDHIT_UGtdEWs}When Troop Knocks Down Formation Captain";
        public const string PenaltyTroopKillsCaptainHeroHint = "{=KDHIT_PWWeMT7}The enemy's troops (all enemy's troops within 20m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 3]";
        public const string PenaltyTroopKillsGeneralHero = "{=KDHIT_6XCQmYB}When Troop Knocks Down General";
        public const string PenaltyTroopKillsGeneralHeroHint = "{=KDHIT_9OFc8j5}The enemy's troops (all enemy's troops within 30m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 4]";
        public const string PenaltyUnassignedHeroKillsUnassignedHero = "{=KDHIT_vznAesO}When Unassigned Hero Knocks Down Unassigned Hero";
        public const string PenaltyUnassignedHeroKillsUnassignedHeroHint = "{=KDHIT_7wBfD7S}The enemy's troops (all enemy's troops within 15m radius by default) suffers a morale loss when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 6]";
        public const string PenaltyUnassignedHeroKillsCaptainHero = "{=KDHIT_I6WcJCc}When Unassigned Hero Knocks Down Formation Captain";
        public const string PenaltyUnassignedHeroKillsCaptainHeroHint = "{=KDHIT_isF6muT}The enemy's troops (all enemy's troops within 25m radius by default) suffers a morale loss when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 7]";
        public const string PenaltyUnassignedHeroKillsGeneralHero = "{=KDHIT_duYOX1f}When Unassigned Hero Knocks Down General";
        public const string PenaltyUnassignedHeroKillsGeneralHeroHint = "{=KDHIT_r78H2kJ}The enemy's troops (all enemy's troops within 35m radius by default) suffers a morale loss when an unassigned hero defeats an enemy general. Works for both sides. [Default: 8]";
        public const string PenaltyCaptainHeroKillsUnassignedHero = "{=KDHIT_UUDtgj1}When Formation Captain Knocks Down Unassigned Hero";
        public const string PenaltyCaptainHeroKillsUnassignedHeroHint = "{=KDHIT_1meRn0j}The enemy's troops (all enemy's troops within 20m radius by default) suffers a morale loss when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 10]";
        public const string PenaltyCaptainHeroKillsCaptainHero = "{=KDHIT_3tkkyDQ}When Formation Captain Knocks Down Formation Captain";
        public const string PenaltyCaptainHeroKillsCaptainHeroHint = "{=KDHIT_7Rtfhw3}The enemy's troops (captain's formation troops) suffers a morale loss when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 11]";
        public const string PenaltyCaptainHeroKillsGeneralHero = "{=KDHIT_DtGsgZo}When Formation Captain Knocks Down General";
        public const string PenaltyCaptainHeroKillsGeneralHeroHint = "{=KDHIT_zeFtZGY}The enemy's troops (all enemy troops) suffers a morale loss when a formation captain defeats an enemy general. Works for both sides. [Default: 12]";
        public const string PenaltyGeneralHeroKillsUnassignedHero = "{=KDHIT_KRVzkuh}When General Knocks Down Unassigned Hero";
        public const string PenaltyGeneralHeroKillsUnassignedHeroHint = "{=KDHIT_fR3QOdQ}The enemy's troops (all enemy's troops within 25m radius by default) suffers a morale loss when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 14]";
        public const string PenaltyGeneralHeroKillsCaptainHero = "{=KDHIT_yD6sszN}When General Knocks Down Formation Captain";
        public const string PenaltyGeneralHeroKillsCaptainHeroHint = "{=KDHIT_LEvkHZs}The enemy's troops (captain's formation troops) suffers a morale loss when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 15]";
        public const string PenaltyGeneralHeroKillsGeneralHero = "{=KDHIT_j4dHqfE}When General Knocks Down General";
        public const string PenaltyGeneralHeroKillsGeneralHeroHint = "{=KDHIT_Q4ntARL}The enemy's troops (all enemy troops) suffers a morale loss when a general defeats an enemy general. Works for both sides. [Default: 16]";

        public const string GroupMoraleGains = "{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down";
        public const string GainTroopKillsUnassignedHero = "{=KDHIT_fKK5mYm}When Troop Knocks Down Unassigned Hero";
        public const string GainTroopKillsUnassignedHeroHint = "{=KDHIT_wPkAX9J}The attacker's troops (all friendly troops within 10m radius by default) gain morale when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 2]";
        public const string GainTroopKillsCaptainHero = "{=KDHIT_XhBHq0E}When Troop Knocks Down Formation Captain";
        public const string GainTroopKillsCaptainHeroHint = "{=KDHIT_em8SzGP}The attacker's troops (all friendly troops within 20m radius by default) gain morale when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 3]";
        public const string GainTroopKillsGeneralHero = "{=KDHIT_PeD32yh}When Troop Knocks Down General";
        public const string GainTroopKillsGeneralHeroHint = "{=KDHIT_DyZp4YZ}The attacker's troops (all friendly troops within 30m radius by default) gain morale when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 4]";
        public const string GainUnassignedHeroKillsUnassignedHero = "{=KDHIT_RteAlHu}When Unassigned Hero Knocks Down Unassigned Hero";
        public const string GainUnassignedHeroKillsUnassignedHeroHint = "{=KDHIT_snw15OQ}The attacker's troops (all friendly troops within 15m radius by default) gain morale when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 6]";
        public const string GainUnassignedHeroKillsCaptainHero = "{=KDHIT_9oNClO7}When Unassigned Hero Knocks Down Formation Captain";
        public const string GainUnassignedHeroKillsCaptainHeroHint = "{=KDHIT_iPTAubL}The attacker's troops (all friendly troops within 25m radius by default) gain morale when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 7]";
        public const string GainUnassignedHeroKillsGeneralHero = "{=KDHIT_mN9WAX3}When Unassigned Hero Knocks Down General";
        public const string GainUnassignedHeroKillsGeneralHeroHint = "{=KDHIT_7Vy0gz0}The attacker's troops (all friendly troops within 35m radius by default) gain morale when an unassigned hero defeats an enemy general. Works for both sides. [Default: 8]";
        public const string GainCaptainHeroKillsUnassignedHero = "{=KDHIT_qgy5fZD}When Formation Captain Knocks Down Unassigned Hero";
        public const string GainCaptainHeroKillsUnassignedHeroHint = "{=KDHIT_PyH89Di}The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 10]";
        public const string GainCaptainHeroKillsCaptainHero = "{=KDHIT_YO4yUEG}When Formation Captain Knocks Down Formation Captain";
        public const string GainCaptainHeroKillsCaptainHeroHint = "{=KDHIT_EhX1Z35}The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 11]";
        public const string GainCaptainHeroKillsGeneralHero = "{=KDHIT_o898cli}When Formation Captain Knocks Down General";
        public const string GainCaptainHeroKillsGeneralHeroHint = "{=KDHIT_NSzvTKl}The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy general. Works for both sides. [Default: 12]";
        public const string GainGeneralHeroKillsUnassignedHero = "{=KDHIT_cAGHoTV}When General Knocks Down Unassigned Hero";
        public const string GainGeneralHeroKillsUnassignedHeroHint = "{=KDHIT_0DyVL1g}The attacker's troops (all friendly troops) gain morale when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 14]";
        public const string GainGeneralHeroKillsCaptainHero = "{=KDHIT_Mr7cbdv}When General Knocks Down Formation Captain";
        public const string GainGeneralHeroKillsCaptainHeroHint = "{=KDHIT_6hBug9f}The attacker's troops (all friendly troops) gain morale when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 15]";
        public const string GainGeneralHeroKillsGeneralHero = "{=KDHIT_Za6yOdt}When General Knocks Down General";
        public const string GainGeneralHeroKillsGeneralHeroHint = "{=KDHIT_kqmtG3J}The attacker's troops (all friendly troops) gain morale when a general defeats an enemy general. Works for both sides. [Default: 16]";

        public const string GroupNotificationTexts = "{=KDHIT_YD0DERb}On-screen notifications texts";
        public const string FriendlyGeneralFallenLabel = "{=KDHIT_iV3XC78}Friendly General Fallen";
        public const string FriendlyGeneralFallenLabelHint = "{=KDHIT_s56GZlT}This text will be shown when friendly general will fall in battle.";
        public const string EnemyGeneralFallenLabel = "{=KDHIT_wVePvMm}Enemy General Fallen";
        public const string EnemyGeneralFallenLabelHint = "{=KDHIT_EeI805q}This text will be shown when enemy general will fall in battle.";
        public const string FriendlyInfantryCaptainFallenLabel = "{=KDHIT_qbFTkze}Friendly Infantry Captain Fallen";
        public const string FriendlyInfantryCaptainFallenLabelHint = "{=KDHIT_UHNgIVw}This text will be shown when friendly infantry captain will fall in battle.";
        public const string FriendlyRangedCaptainFallenLabel = "{=KDHIT_WoZPz6c}Friendly Ranged Captain Fallen";
        public const string FriendlyRangedCaptainFallenLabelHint = "{=KDHIT_oVJ3eRy}This text will be shown when friendly ranged captain will fall in battle.";
        public const string FriendlyCavalryCaptainFallenLabel = "{=KDHIT_b8AAr0U}Friendly Cavalry Captain Fallen";
        public const string FriendlyCavalryCaptainFallenLabelHint = "{=KDHIT_AQNuxE5}This text will be shown when friendly cavalry captain will fall in battle.";
        public const string FriendlyHorseArchersCaptainFallenLabel = "{=KDHIT_s5VAzgZ}Friendly Horse Archers Captain Fallen";
        public const string FriendlyHorseArchersCaptainFallenLabelHint = "{=KDHIT_xoLedGz}This text will be shown when friendly horse archers captain will fall in battle.";
        public const string EnemyInfantryCaptainFallenLabel = "{=KDHIT_6WCwIwf}Enemy Infantry Captain Fallen";
        public const string EnemyInfantryCaptainFallenLabelHint = "{=KDHIT_s4PzqoL}This text will be shown when enemy's infantry captain will fall in battle.";
        public const string EnemyRangedCaptainFallenLabel = "{=KDHIT_FSznMXB}Enemy Ranged Captain Fallen";
        public const string EnemyRangedCaptainFallenLabelHint = "{=KDHIT_E5blIa4}This text will be shown when enemy's ranged captain will fall in battle.";
        public const string EnemyCavalryCaptainFallenLabel = "{=KDHIT_Q6uPIqN}Enemy Cavalry Captain Fallen";
        public const string EnemyCavalryCaptainFallenLabelHint = "{=KDHIT_LTMUzWd}This text will be shown when enemy's cavalry captain will fall in battle.";
        public const string EnemyHorseArchersCaptainFallenLabel = "{=KDHIT_NASm63y}Enemy Horse Archers Captain Fallen";
        public const string EnemyHorseArchersCaptainFallenLabelHint = "{=KDHIT_ZZwZSL9}This text will be shown when enemy's horse archers captain will fall in battle.";
        public const string FriendlyUnassignedHeroFallenLabel = "{=KDHIT_Q6yrpIe}Friendly Unassigned Hero Fallen";
        public const string FriendlyUnassignedHeroFallenLabelHint = "{=KDHIT_3q3G6R5}This text will be shown when friendly unassigned hero will fall in battle.";
        public const string EnemyUnassignedHeroFallenLabel = "{=KDHIT_otYAcfm}Enemy Unassigned Hero Fallen";
        public const string EnemyUnassignedHeroFallenLabelHint = "{=KDHIT_tCIe7Fw}This text will be shown when enemy's unassigned hero will fall in battle.";

        public const string GroupTechnical = "{=KDHIT_IG2AH0v}Technical settings";
        public const string UpdateIntervalInSeconds = "{=KDHIT_Aei3QtZ}Mod update interval in seconds";
        public const string UpdateIntervalInSecondsHint = "{=KDHIT_4jgoXLb}This mod updates hero and troop lists every XX seconds. It doesn’t need to be changed. [Default: 30]";
        public const string LoggingEnabled = "{=KDHIT_AGgXl75}Logging for debugging";
        public const string LoggingEnabledHint = "{=KDHIT_BQhlZPM}Logging for debugging. [Default: disabled]";
        #endregion

        #region Notifications
        // The English text of the 12 editable notifications is their setting default in ModConfiguration.
        public const string FriendlyGeneralFallen = "{=KDHIT_57qRnbg}" + ModConfiguration.FriendlyGeneralFallenNotificationDefault;
        public const string EnemyGeneralFallen = "{=KDHIT_YxuK6fA}" + ModConfiguration.EnemyGeneralFallenNotificationDefault;
        public const string FriendlyInfantryCaptainFallen = "{=KDHIT_ABEqr3n}" + ModConfiguration.FriendlyInfantryCaptainFallenNotificationDefault;
        public const string FriendlyRangedCaptainFallen = "{=KDHIT_Amerh0t}" + ModConfiguration.FriendlyRangedCaptainFallenNotificationDefault;
        public const string FriendlyCavalryCaptainFallen = "{=KDHIT_8Fzqb4c}" + ModConfiguration.FriendlyCavalryCaptainFallenNotificationDefault;
        public const string FriendlyHorseArchersCaptainFallen = "{=KDHIT_JhWooWH}" + ModConfiguration.FriendlyHorseArchersCaptainFallenNotificationDefault;
        public const string EnemyInfantryCaptainFallen = "{=KDHIT_NZ7KC8a}" + ModConfiguration.EnemyInfantryCaptainFallenNotificationDefault;
        public const string EnemyRangedCaptainFallen = "{=KDHIT_zfGD6ad}" + ModConfiguration.EnemyRangedCaptainFallenNotificationDefault;
        public const string EnemyCavalryCaptainFallen = "{=KDHIT_BTTw8p6}" + ModConfiguration.EnemyCavalryCaptainFallenNotificationDefault;
        public const string EnemyHorseArchersCaptainFallen = "{=KDHIT_Q8exFRD}" + ModConfiguration.EnemyHorseArchersCaptainFallenNotificationDefault;
        public const string FriendlyUnassignedHeroFallen = "{=KDHIT_xJFUXi0}" + ModConfiguration.FriendlyUnassignedHeroFallenNotificationDefault;
        public const string EnemyUnassignedHeroFallen = "{=KDHIT_EfBnjgz}" + ModConfiguration.EnemyUnassignedHeroFallenNotificationDefault;

        // Shown when a fallen captain's formation type is unknown.
        public const string FriendlyCaptainFallen = "{=KDHIT_nlFILgW}Your captain has fallen! Give up!";
        public const string EnemyCaptainFallen = "{=KDHIT_NQjIwCQ}Enemy's captain has fallen! Good fight!";
        #endregion
    }
}
