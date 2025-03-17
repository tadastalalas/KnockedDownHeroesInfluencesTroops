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
        { get { return new TextObject("{=KDHIT_zSNYn1}Knocked Down Heroes Influences Troops").ToString(); } }

        public override string FolderName
        { get { return "KnockedDownHeroesInfluencesTroops"; } }

        public override string FormatType
        { get { return "json2"; } }

        [SettingPropertyInteger("{=KDHIT_nTwCqB}When Troop Knocks Down Unassigned Hero", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "{=KDHIT_xMVk0a}The enemy suffers a morale loss when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 4]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenTroopKillsUnassignedHero { get; set; } = 4;

        [SettingPropertyInteger("{=KDHIT_ck8DCA}When Troop Knocks Down Formation Captain", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "{=KDHIT_gONKNw}The enemy suffers a morale loss when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 6]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenTroopKillsCaptainHero { get; set; } = 6;

        [SettingPropertyInteger("{=KDHIT_7ft8BC}When Troop Knocks Down General", 0, 100, "0", Order = 2, RequireRestart = false, HintText = "{=KDHIT_iYE63d}The enemy suffers a morale loss when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 8]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenTroopKillsGeneralHero { get; set; } = 8;

        [SettingPropertyInteger("{=KDHIT_GsOvcd}When Unassigned Hero Knocks Down Unassigned Hero", 0, 100, "0", Order = 3, RequireRestart = false, HintText = "{=KDHIT_5SxZvI}The enemy suffers a morale loss when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 12]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenUnassignedHeroKillsUnassignedHero { get; set; } = 12;

        [SettingPropertyInteger("{=KDHIT_rJu3Ta}When Unassigned Hero Knocks Down Formation Captain", 0, 100, "0", Order = 4, RequireRestart = false, HintText = "{=KDHIT_27M1Cw}The enemy suffers a morale loss when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 14]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenUnassignedHeroKillsCaptainHero { get; set; } = 14;

        [SettingPropertyInteger("{=KDHIT_fuQ7uL}When Unassigned Hero Knocks Down General", 0, 100, "0", Order = 5, RequireRestart = false, HintText = "{=KDHIT_Uy9idp}The enemy suffers a morale loss when an unassigned hero defeats an enemy general. Works for both sides. [Default: 16]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenUnassignedHeroKillsGeneralHero { get; set; } = 16;

        [SettingPropertyInteger("{=KDHIT_bCwaLZ}When Formation Captain Knocks Down Unassigned Hero", 0, 100, "0", Order = 6, RequireRestart = false, HintText = "{=KDHIT_u6igdF}The enemy suffers a morale loss when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 20]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenCaptainHeroKillsUnassignedHero { get; set; } = 20;

        [SettingPropertyInteger("{=KDHIT_ihF98p}When Formation Captain Knocks Down Formation Captain", 0, 100, "0", Order = 7, RequireRestart = false, HintText = "{=KDHIT_wbtwEt}The enemy suffers a morale loss when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 22]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenCaptainHeroKillsCaptainHero { get; set; } = 22;

        [SettingPropertyInteger("{=KDHIT_v39avE}When Formation Captain Knocks Down General", 0, 100, "0", Order = 8, RequireRestart = false, HintText = "{=KDHIT_H23Aqm}The enemy suffers a morale loss when a formation captain defeats an enemy general. Works for both sides. [Default: 24]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenCaptainHeroKillsGeneralHero { get; set; } = 24;

        [SettingPropertyInteger("{=KDHIT_Ub2Yr1}When General Knocks Down Unassigned Hero", 0, 100, "0", Order = 9, RequireRestart = false, HintText = "{=KDHIT_3ufjIe}The enemy suffers a morale loss when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 28]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenGeneralHeroKillsUnassignedHero { get; set; } = 28;

        [SettingPropertyInteger("{=KDHIT_Ak8sE0}When General Knocks Down Formation Captain", 0, 100, "0", Order = 10, RequireRestart = false, HintText = "{=KDHIT_XJexA4}The enemy suffers a morale loss when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 30]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenGeneralHeroKillsCaptainHero { get; set; } = 30;

        [SettingPropertyInteger("{=KDHIT_DMQ1mL}When General Knocks Down General", 0, 100, "0", Order = 11, RequireRestart = false, HintText = "{=KDHIT_9PRkKj}The enemy suffers a morale loss when a genral defeats an enemy general. Works for both sides. [Default: 32]")]
        [SettingPropertyGroup("{=KDHIT_moZQWd}Morale penalties when heroes are knocked down", GroupOrder = 0)]
        public int MoraleChangeWhenGeneralHeroKillsGeneralHero { get; set; } = 32;

        [SettingPropertyBool("Show On-Screen Notifications", Order = 0, RequireRestart = false, HintText = "Toggle on-screen notifications when any lord is knocked down. [Default: enabled]")]
        [SettingPropertyGroup("{=KDHIT_bYRqo3}Technical settings", GroupOrder = 1)]
        public bool ShowOnScreenNotifications { get; set; } = true;

        [SettingPropertyInteger("{=KDHIT_x1Cnfc}Mod update interval in seconds", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "{=KDHIT_aFxaeQ}This mod updates hero and troop lists every XX seconds. It doesn’t need to be changed. [Default: 30]")]
        [SettingPropertyGroup("{=KDHIT_bYRqo3}Technical settings", GroupOrder = 1)]
        public int UpdateIntervalInSeconds { get; set; } = 30;

        [SettingPropertyBool("{=KDHIT_u4S4Ha}Logging for debugging", Order = 2, RequireRestart = false, HintText = "{=KDHIT_ZXA7JS}Logging for debugging. [Default: disabled]")]
        [SettingPropertyGroup("{=KDHIT_bYRqo3}Technical settings", GroupOrder = 1)]
        public bool LoggingEnabled { get; set; } = false;
    }
}