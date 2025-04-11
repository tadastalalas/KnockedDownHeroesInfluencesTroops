using MCM.Abstractions.Attributes.v2;
using MCM.Abstractions.Attributes;
using MCM.Abstractions.Base.Global;
using TaleWorlds.Localization;

namespace KnockedDownHeroesInfluencesTroops
{
    internal class MCMSettings : AttributeGlobalSettings<MCMSettings>
    {
        public override string Id
        { get { return "KnockedDownHeroesInfluencesTroopsSettings"; } }

        public override string DisplayName
        { get { return new TextObject("{=KDHIT_yz6lxdJ}Knocked Down Heroes Influences Troops").ToString(); } }

        public override string FolderName
        { get { return "KnockedDownHeroesInfluencesTroops"; } }

        public override string FormatType
        { get { return "json2"; } }

        // Main settings

        [SettingPropertyBool("{=KDHIT_4qcKDe7}Enable This Modification", Order = 0, RequireRestart = false, HintText = "{=KDHIT_ouqvaSg}Toggle this modification. [Default: enabled]")]
        [SettingPropertyGroup("{=KDHIT_AjEK8im}Main settings", GroupOrder = 0)]
        public bool EnableThisModification { get; set; } = true;

        [SettingPropertyBool("{=KDHIT_bJQyNOn}Show On-Screen Notifications", Order = 1, RequireRestart = false, HintText = "{=KDHIT_2EJdYrV}Toggle on-screen notifications when any hero is knocked down. [Default: enabled]")]
        [SettingPropertyGroup("{=KDHIT_AjEK8im}Main settings", GroupOrder = 0)]
        public bool ShowOnScreenNotifications { get; set; } = true;

        // Morale penalties when heroes are knocked down

        [SettingPropertyInteger("{=KDHIT_c4srbaQ}When Troop Knocks Down Unassigned Hero", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "{=KDHIT_tyxCqZv}The enemy's troops (all enemy's troops within 10m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 2]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenTroopKillsUnassignedHero { get; set; } = 2;

        [SettingPropertyInteger("{=KDHIT_UGtdEWs}When Troop Knocks Down Formation Captain", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "{=KDHIT_PWWeMT7}The enemy's troops (all enemy's troops within 20m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 3]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenTroopKillsCaptainHero { get; set; } = 3;

        [SettingPropertyInteger("{=KDHIT_6XCQmYB}When Troop Knocks Down General", 0, 100, "0", Order = 2, RequireRestart = false, HintText = "{=KDHIT_9OFc8j5}The enemy's troops (all enemy's troops within 30m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 4]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenTroopKillsGeneralHero { get; set; } = 4;

        [SettingPropertyInteger("{=KDHIT_vznAesO}When Unassigned Hero Knocks Down Unassigned Hero", 0, 100, "0", Order = 3, RequireRestart = false, HintText = "{=KDHIT_7wBfD7S}The enemy's troops (all enemy's troops within 15m radius by default) suffers a morale loss when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 6]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenUnassignedHeroKillsUnassignedHero { get; set; } = 6;

        [SettingPropertyInteger("{=KDHIT_I6WcJCc}When Unassigned Hero Knocks Down Formation Captain", 0, 100, "0", Order = 4, RequireRestart = false, HintText = "{=KDHIT_isF6muT}The enemy's troops (all enemy's troops within 25m radius by default) suffers a morale loss when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 7]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenUnassignedHeroKillsCaptainHero { get; set; } = 7;

        [SettingPropertyInteger("{=KDHIT_duYOX1f}When Unassigned Hero Knocks Down General", 0, 100, "0", Order = 5, RequireRestart = false, HintText = "{=KDHIT_r78H2kJ}The enemy's troops (all enemy's troops within 35m radius by default) suffers a morale loss when an unassigned hero defeats an enemy general. Works for both sides. [Default: 8]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenUnassignedHeroKillsGeneralHero { get; set; } = 8;

        [SettingPropertyInteger("{=KDHIT_UUDtgj1}When Formation Captain Knocks Down Unassigned Hero", 0, 100, "0", Order = 6, RequireRestart = false, HintText = "{=KDHIT_1meRn0j}The enemy's troops (all enemy's troops within 20m radius by default) suffers a morale loss when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 10]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenCaptainHeroKillsUnassignedHero { get; set; } = 10;

        [SettingPropertyInteger("{=KDHIT_3tkkyDQ}When Formation Captain Knocks Down Formation Captain", 0, 100, "0", Order = 7, RequireRestart = false, HintText = "{=KDHIT_7Rtfhw3}The enemy's troops (captain's formation troops) suffers a morale loss when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 11]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenCaptainHeroKillsCaptainHero { get; set; } = 11;

        [SettingPropertyInteger("{=KDHIT_DtGsgZo}When Formation Captain Knocks Down General", 0, 100, "0", Order = 8, RequireRestart = false, HintText = "{=KDHIT_zeFtZGY}The enemy's troops (all enemy troops) suffers a morale loss when a formation captain defeats an enemy general. Works for both sides. [Default: 12]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenCaptainHeroKillsGeneralHero { get; set; } = 12;

        [SettingPropertyInteger("{=KDHIT_KRVzkuh}When General Knocks Down Unassigned Hero", 0, 100, "0", Order = 9, RequireRestart = false, HintText = "{=KDHIT_fR3QOdQ}The enemy's troops (all enemy's troops within 25m radius by default) suffers a morale loss when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 14]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenGeneralHeroKillsUnassignedHero { get; set; } = 14;

        [SettingPropertyInteger("{=KDHIT_yD6sszN}When General Knocks Down Formation Captain", 0, 100, "0", Order = 10, RequireRestart = false, HintText = "{=KDHIT_LEvkHZs}The enemy's troops (captain's formation troops) suffers a morale loss when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 15]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenGeneralHeroKillsCaptainHero { get; set; } = 15;

        [SettingPropertyInteger("{=KDHIT_j4dHqfE}When General Knocks Down General", 0, 100, "0", Order = 11, RequireRestart = false, HintText = "{=KDHIT_Q4ntARL}The enemy's troops (all enemy troops) suffers a morale loss when a genral defeats an enemy general. Works for both sides. [Default: 16]")]
        [SettingPropertyGroup("{=KDHIT_gfKSI8d}Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenGeneralHeroKillsGeneralHero { get; set; } = 16;

        // Morale gains when heroes are knocked down

        [SettingPropertyInteger("{=KDHIT_fKK5mYm}When Troop Knocks Down Unassigned Hero", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "{=KDHIT_wPkAX9J}The attacker's troops (all friendly troops within 10m radius by default) gain morale when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 2]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenTroopKillsUnassignedHero { get; set; } = 2;

        [SettingPropertyInteger("{=KDHIT_XhBHq0E}When Troop Knocks Down Formation Captain", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "{=KDHIT_em8SzGP}The attacker's troops (all friendly troops within 20m radius by default) gain morale when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 3]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenTroopKillsCaptainHero { get; set; } = 3;

        [SettingPropertyInteger("{=KDHIT_PeD32yh}When Troop Knocks Down General", 0, 100, "0", Order = 2, RequireRestart = false, HintText = "{=KDHIT_DyZp4YZ}The attacker's troops (all friendly troops within 30m radius by default) gain morale when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 4]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenTroopKillsGeneralHero { get; set; } = 4;

        [SettingPropertyInteger("{=KDHIT_RteAlHu}When Unassigned Hero Knocks Down Unassigned Hero", 0, 100, "0", Order = 3, RequireRestart = false, HintText = "{=KDHIT_snw15OQ}The attacker's troops (all friendly troops within 15m radius by default) gain morale when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 6]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenUnassignedHeroKillsUnassignedHero { get; set; } = 6;

        [SettingPropertyInteger("{=KDHIT_9oNClO7}When Unassigned Hero Knocks Down Formation Captain", 0, 100, "0", Order = 4, RequireRestart = false, HintText = "{=KDHIT_iPTAubL}The attacker's troops (all friendly troops within 25m radius by default) gain morale when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 7]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenUnassignedHeroKillsCaptainHero { get; set; } = 7;

        [SettingPropertyInteger("{=KDHIT_mN9WAX3}When Unassigned Hero Knocks Down General", 0, 100, "0", Order = 5, RequireRestart = false, HintText = "{=KDHIT_7Vy0gz0}The attacker's troops (all friendly troops within 35m radius by default) gain morale when an unassigned hero defeats an enemy general. Works for both sides. [Default: 8]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenUnassignedHeroKillsGeneralHero { get; set; } = 8;

        [SettingPropertyInteger("{=KDHIT_qgy5fZD}When Formation Captain Knocks Down Unassigned Hero", 0, 100, "0", Order = 6, RequireRestart = false, HintText = "{=KDHIT_PyH89Di}The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 10]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenCaptainHeroKillsUnassignedHero { get; set; } = 10;

        [SettingPropertyInteger("{=KDHIT_YO4yUEG}When Formation Captain Knocks Down Formation Captain", 0, 100, "0", Order = 7, RequireRestart = false, HintText = "{=KDHIT_EhX1Z35}The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 11]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenCaptainHeroKillsCaptainHero { get; set; } = 11;

        [SettingPropertyInteger("{=KDHIT_o898cli}When Formation Captain Knocks Down General", 0, 100, "0", Order = 8, RequireRestart = false, HintText = "{=KDHIT_NSzvTKl}The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy general. Works for both sides. [Default: 12]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenCaptainHeroKillsGeneralHero { get; set; } = 12;

        [SettingPropertyInteger("{=KDHIT_cAGHoTV}When General Knocks Down Unassigned Hero", 0, 100, "0", Order = 9, RequireRestart = false, HintText = "{=KDHIT_0DyVL1g}The attacker's troops (all friendly troops) gain morale when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 14]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenGeneralHeroKillsUnassignedHero { get; set; } = 14;

        [SettingPropertyInteger("{=KDHIT_Mr7cbdv}When General Knocks Down Formation Captain", 0, 100, "0", Order = 10, RequireRestart = false, HintText = "{=KDHIT_6hBug9f}The attacker's troops (all friendly troops) gain morale when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 15]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenGeneralHeroKillsCaptainHero { get; set; } = 15;

        [SettingPropertyInteger("{=KDHIT_Za6yOdt}When General Knocks Down General", 0, 100, "0", Order = 11, RequireRestart = false, HintText = "{=KDHIT_kqmtG3J}The attacker's troops (all friendly troops) gain morale when a genral defeats an enemy general. Works for both sides. [Default: 16]")]
        [SettingPropertyGroup("{=KDHIT_MdLvOaI}Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenGeneralHeroKillsGeneralHero { get; set; } = 16;

        // On-screen notifications texts

        [SettingPropertyText("{=KDHIT_iV3XC78}Friendly General Fallen", -1, true, "", Order = 0, RequireRestart = false, HintText = "{=KDHIT_s56GZlT}This text will be shown when friendly general will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string friendlyGeneralFallenNotification { get; set; } = "Your general has fallen! Nobody will save you!";

        [SettingPropertyText("{=KDHIT_wVePvMm}Enemy General Fallen", -1, true, "", Order = 1, RequireRestart = false, HintText = "{=KDHIT_EeI805q}This text will be shown when enemy general will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string enemyGeneralFallenNotification { get; set; } = "Enemy's general has fallen! Victory is ours!";

        [SettingPropertyText("{=KDHIT_qbFTkze}Friendly Infantry Captain Fallen", -1, true, "", Order = 2, RequireRestart = false, HintText = "{=KDHIT_UHNgIVw}This text will be shown when friendly infantry captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string friendlyInfantryCaptainFallenNotification { get; set; } = "Your infantry captain has fallen! Give up!";

        [SettingPropertyText("{=KDHIT_WoZPz6c}Friendly Ranged Captain Fallen", -1, true, "", Order = 3, RequireRestart = false, HintText = "{=KDHIT_oVJ3eRy}This text will be shown when friendly ranged captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string friendlyRangedCaptainFallenNotification { get; set; } = "Your ranged captain has fallen! Give up!";

        [SettingPropertyText("{=KDHIT_b8AAr0U}Friendly Cavalry Captain Fallen", -1, true, "", Order = 4, RequireRestart = false, HintText = "{=KDHIT_AQNuxE5}This text will be shown when friendly cavalry captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string friendlyCavalryCaptainFallenNotification { get; set; } = "Your cavalry captain has fallen! Give up!";

        [SettingPropertyText("{=KDHIT_s5VAzgZ}Friendly Horse Archers Captain Fallen", -1, true, "", Order = 5, RequireRestart = false, HintText = "{=KDHIT_xoLedGz}This text will be shown when friendly horse archers captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string friendlyHorseArchersCaptainFallenNotification { get; set; } = "Your horse archers captain has fallen! Give up!";

        [SettingPropertyText("{=KDHIT_6WCwIwf}Enemy Infantry Captain Fallen", -1, true, "", Order = 6, RequireRestart = false, HintText = "{=KDHIT_s4PzqoL}This text will be shown when enemy's infantry captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string enemyInfantryCaptainFallenNotification { get; set; } = "Enemy's infantry captain has fallen! Good fight!";

        [SettingPropertyText("{=KDHIT_FSznMXB}Enemy Ranged Captain Fallen", -1, true, "", Order = 7, RequireRestart = false, HintText = "{=KDHIT_E5blIa4}This text will be shown when enemy's ranged captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string enemyRangedCaptainFallenNotification { get; set; } = "Enemy's archers captain has fallen! Good fight!";

        [SettingPropertyText("{=KDHIT_Q6uPIqN}Enemy Cavalry Captain Fallen", -1, true, "", Order = 8, RequireRestart = false, HintText = "{=KDHIT_LTMUzWd}This text will be shown when enemy's cavalry captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string enemyCavalryCaptainFallenNotification { get; set; } = "Enemy's cavalry captain has fallen! Good fight!";

        [SettingPropertyText("{=KDHIT_NASm63y}Enemy Horse Archers Captain Fallen", -1, true, "", Order = 9, RequireRestart = false, HintText = "{=KDHIT_ZZwZSL9}This text will be shown when enemy's horse archers captain will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string enemyHorseArchersCaptainFallenNotification { get; set; } = "Enemy's horse archers captain has fallen! Good fight!";

        [SettingPropertyText("{=KDHIT_Q6yrpIe}Friendly Unassigned Hero Fallen", -1, true, "", Order = 10, RequireRestart = false, HintText = "{=KDHIT_3q3G6R5}This text will be shown when friendly unassigned hero will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string friendlyUnassignedHeroFallenNotification { get; set; } = "One of your heroes has fallen!";

        [SettingPropertyText("{=KDHIT_otYAcfm}Enemy Unassigned Hero Fallen", -1, true, "", Order = 11, RequireRestart = false, HintText = "{=KDHIT_tCIe7Fw}This text will be shown when enemy's unassigned hero will fall in battle.")]
        [SettingPropertyGroup("{=KDHIT_YD0DERb}On-screen notifications texts", GroupOrder = 3)]
        public string enemyUnassignedHeroFallenNotification { get; set; } = "One of their heroes has fallen!";

        // Technical settings

        [SettingPropertyInteger("{=KDHIT_Aei3QtZ}Mod update interval in seconds", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "{=KDHIT_4jgoXLb}This mod updates hero and troop lists every XX seconds. It doesn’t need to be changed. [Default: 30]")]
        [SettingPropertyGroup("{=KDHIT_IG2AH0v}Technical settings", GroupOrder = 4)]
        public int UpdateIntervalInSeconds { get; set; } = 30;

        [SettingPropertyBool("{=KDHIT_AGgXl75}Logging for debugging", Order = 2, RequireRestart = false, HintText = "{=KDHIT_BQhlZPM}Logging for debugging. [Default: disabled]")]
        [SettingPropertyGroup("{=KDHIT_IG2AH0v}Technical settings", GroupOrder = 4)]
        public bool LoggingEnabled { get; set; } = false;
    }
}