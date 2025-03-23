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
        { get { return new TextObject("Knocked Down Heroes Influences Troops").ToString(); } }

        public override string FolderName
        { get { return "KnockedDownHeroesInfluencesTroops"; } }

        public override string FormatType
        { get { return "json2"; } }

        // Main settings

        [SettingPropertyBool("Enable This Modification", Order = 0, RequireRestart = false, HintText = "Toggle this modification. [Default: enabled]")]
        [SettingPropertyGroup("Main settings", GroupOrder = 0)]
        public bool EnableThisModification { get; set; } = true;

        [SettingPropertyBool("Show On-Screen Notifications", Order = 1, RequireRestart = false, HintText = "Toggle on-screen notifications when any hero is knocked down. [Default: enabled]")]
        [SettingPropertyGroup("Main settings", GroupOrder = 0)]
        public bool ShowOnScreenNotifications { get; set; } = true;

        // Morale penalties when heroes are knocked down

        [SettingPropertyInteger("When Troop Knocks Down Unassigned Hero", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 10m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 2]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenTroopKillsUnassignedHero { get; set; } = 2;

        [SettingPropertyInteger("When Troop Knocks Down Formation Captain", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 20m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 3]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenTroopKillsCaptainHero { get; set; } = 3;

        [SettingPropertyInteger("When Troop Knocks Down General", 0, 100, "0", Order = 2, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 30m radius by default) suffers a morale loss when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 4]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenTroopKillsGeneralHero { get; set; } = 4;

        [SettingPropertyInteger("When Unassigned Hero Knocks Down Unassigned Hero", 0, 100, "0", Order = 3, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 15m radius by default) suffers a morale loss when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 6]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenUnassignedHeroKillsUnassignedHero { get; set; } = 6;

        [SettingPropertyInteger("When Unassigned Hero Knocks Down Formation Captain", 0, 100, "0", Order = 4, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 25m radius by default) suffers a morale loss when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 7]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenUnassignedHeroKillsCaptainHero { get; set; } = 7;

        [SettingPropertyInteger("When Unassigned Hero Knocks Down General", 0, 100, "0", Order = 5, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 35m radius by default) suffers a morale loss when an unassigned hero defeats an enemy general. Works for both sides. [Default: 8]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenUnassignedHeroKillsGeneralHero { get; set; } = 8;

        [SettingPropertyInteger("When Formation Captain Knocks Down Unassigned Hero", 0, 100, "0", Order = 6, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 20m radius by default) suffers a morale loss when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 10]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenCaptainHeroKillsUnassignedHero { get; set; } = 10;

        [SettingPropertyInteger("When Formation Captain Knocks Down Formation Captain", 0, 100, "0", Order = 7, RequireRestart = false, HintText = "The enemy's troops (captain's formation troops) suffers a morale loss when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 11]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenCaptainHeroKillsCaptainHero { get; set; } = 11;

        [SettingPropertyInteger("When Formation Captain Knocks Down General", 0, 100, "0", Order = 8, RequireRestart = false, HintText = "The enemy's troops (all enemy troops) suffers a morale loss when a formation captain defeats an enemy general. Works for both sides. [Default: 12]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenCaptainHeroKillsGeneralHero { get; set; } = 12;

        [SettingPropertyInteger("When General Knocks Down Unassigned Hero", 0, 100, "0", Order = 9, RequireRestart = false, HintText = "The enemy's troops (all enemy's troops within 25m radius by default) suffers a morale loss when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 14]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenGeneralHeroKillsUnassignedHero { get; set; } = 14;

        [SettingPropertyInteger("When General Knocks Down Formation Captain", 0, 100, "0", Order = 10, RequireRestart = false, HintText = "The enemy's troops (captain's formation troops) suffers a morale loss when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 15]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenGeneralHeroKillsCaptainHero { get; set; } = 15;

        [SettingPropertyInteger("When General Knocks Down General", 0, 100, "0", Order = 11, RequireRestart = false, HintText = "The enemy's troops (all enemy troops) suffers a morale loss when a genral defeats an enemy general. Works for both sides. [Default: 16]")]
        [SettingPropertyGroup("Morale penalties when heroes are knocked down", GroupOrder = 1)]
        public int MoraleChangeWhenGeneralHeroKillsGeneralHero { get; set; } = 16;

        // Morale gains when heroes are knocked down

        [SettingPropertyInteger("When Troop Knocks Down Unassigned Hero", 0, 100, "0", Order = 0, RequireRestart = false, HintText = "The attacker's troops (all friendly troops within 10m radius by default) gain morale when an ordinary soldier defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 2]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenTroopKillsUnassignedHero { get; set; } = 2;

        [SettingPropertyInteger("When Troop Knocks Down Formation Captain", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "The attacker's troops (all friendly troops within 20m radius by default) gain morale when an ordinary soldier defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 3]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenTroopKillsCaptainHero { get; set; } = 3;

        [SettingPropertyInteger("When Troop Knocks Down General", 0, 100, "0", Order = 2, RequireRestart = false, HintText = "The attacker's troops (all friendly troops within 30m radius by default) gain morale when an ordinary soldier defeats an enemy general. Works for both sides. [Default: 4]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenTroopKillsGeneralHero { get; set; } = 4;

        [SettingPropertyInteger("When Unassigned Hero Knocks Down Unassigned Hero", 0, 100, "0", Order = 3, RequireRestart = false, HintText = "The attacker's troops (all friendly troops within 15m radius by default) gain morale when an unassigned hero defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 6]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenUnassignedHeroKillsUnassignedHero { get; set; } = 6;

        [SettingPropertyInteger("When Unassigned Hero Knocks Down Formation Captain", 0, 100, "0", Order = 4, RequireRestart = false, HintText = "The attacker's troops (all friendly troops within 25m radius by default) gain morale when an unassigned hero defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 7]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenUnassignedHeroKillsCaptainHero { get; set; } = 7;

        [SettingPropertyInteger("When Unassigned Hero Knocks Down General", 0, 100, "0", Order = 5, RequireRestart = false, HintText = "The attacker's troops (all friendly troops within 35m radius by default) gain morale when an unassigned hero defeats an enemy general. Works for both sides. [Default: 8]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenUnassignedHeroKillsGeneralHero { get; set; } = 8;

        [SettingPropertyInteger("When Formation Captain Knocks Down Unassigned Hero", 0, 100, "0", Order = 6, RequireRestart = false, HintText = "The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 10]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenCaptainHeroKillsUnassignedHero { get; set; } = 10;

        [SettingPropertyInteger("When Formation Captain Knocks Down Formation Captain", 0, 100, "0", Order = 7, RequireRestart = false, HintText = "The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 11]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenCaptainHeroKillsCaptainHero { get; set; } = 11;

        [SettingPropertyInteger("When Formation Captain Knocks Down General", 0, 100, "0", Order = 8, RequireRestart = false, HintText = "The attacker's troops (captain's formation troops) gain morale when a formation captain defeats an enemy general. Works for both sides. [Default: 12]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenCaptainHeroKillsGeneralHero { get; set; } = 12;

        [SettingPropertyInteger("When General Knocks Down Unassigned Hero", 0, 100, "0", Order = 9, RequireRestart = false, HintText = "The attacker's troops (all friendly troops) gain morale when a general defeats an enemy hero not serving as a formation captain. Works for both sides. [Default: 14]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenGeneralHeroKillsUnassignedHero { get; set; } = 14;

        [SettingPropertyInteger("When General Knocks Down Formation Captain", 0, 100, "0", Order = 10, RequireRestart = false, HintText = "The attacker's troops (all friendly troops) gain morale when a general defeats an enemy hero serving as a formation captain. Works for both sides. [Default: 15]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenGeneralHeroKillsCaptainHero { get; set; } = 15;

        [SettingPropertyInteger("When General Knocks Down General", 0, 100, "0", Order = 11, RequireRestart = false, HintText = "The attacker's troops (all friendly troops) gain morale when a genral defeats an enemy general. Works for both sides. [Default: 16]")]
        [SettingPropertyGroup("Morale gains when heroes are knocked down", GroupOrder = 2)]
        public int MoraleGainWhenGeneralHeroKillsGeneralHero { get; set; } = 16;

        // On-screen notifications texts

        [SettingPropertyText("Friendly General Fallen", -1, true, "", Order = 0, RequireRestart = false, HintText = "This text will be shown when friendly general will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string friendlyGeneralFallenNotification { get; set; } = "Your general has fallen! Nobody will save you!";

        [SettingPropertyText("Enemy General Fallen", -1, true, "", Order = 1, RequireRestart = false, HintText = "This text will be shown when enemy general will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string enemyGeneralFallenNotification { get; set; } = "Enemy's general has fallen! Victory is ours!";

        [SettingPropertyText("Friendly Infantry Captain Fallen", -1, true, "", Order = 2, RequireRestart = false, HintText = "This text will be shown when friendly infantry captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string friendlyInfantryCaptainFallenNotification { get; set; } = "Your infantry captain has fallen! Give up!";

        [SettingPropertyText("Friendly Ranged Captain Fallen", -1, true, "", Order = 3, RequireRestart = false, HintText = "This text will be shown when friendly ranged captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string friendlyRangedCaptainFallenNotification { get; set; } = "Your ranged captain has fallen! Give up!";

        [SettingPropertyText("Friendly Cavalry Captain Fallen", -1, true, "", Order = 4, RequireRestart = false, HintText = "This text will be shown when friendly cavalry captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string friendlyCavalryCaptainFallenNotification { get; set; } = "Your cavalry captain has fallen! Give up!";

        [SettingPropertyText("Friendly Horse Archers Captain Fallen", -1, true, "", Order = 5, RequireRestart = false, HintText = "This text will be shown when friendly horse archers captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string friendlyHorseArchersCaptainFallenNotification { get; set; } = "Your horse archers captain has fallen! Attack!";

        [SettingPropertyText("Enemy Infantry Captain Fallen", -1, true, "", Order = 6, RequireRestart = false, HintText = "This text will be shown when enemy's infantry captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string enemyInfantryCaptainFallenNotification { get; set; } = "Enemy's infantry captain has fallen! Good fight!";

        [SettingPropertyText("Enemy Ranged Captain Fallen", -1, true, "", Order = 7, RequireRestart = false, HintText = "This text will be shown when enemy's ranged captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string enemyRangedCaptainFallenNotification { get; set; } = "Enemy's archers captain has fallen! Good fight!";

        [SettingPropertyText("Enemy Cavalry Captain Fallen", -1, true, "", Order = 8, RequireRestart = false, HintText = "This text will be shown when enemy's cavalry captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string enemyCavalryCaptainFallenNotification { get; set; } = "Enemy's cavalry captain has fallen! Good fight!";

        [SettingPropertyText("Enemy Horse Archers Captain Fallen", -1, true, "", Order = 9, RequireRestart = false, HintText = "This text will be shown when enemy's horse archers captain will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string enemyHorseArchersCaptainFallenNotification { get; set; } = "Enemy's horse archers captain has fallen! Good fight!";

        [SettingPropertyText("Friendly Unassigned Hero Fallen", -1, true, "", Order = 10, RequireRestart = false, HintText = "This text will be shown when friendly unassigned hero will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string friendlyUnassignedHeroFallenNotification { get; set; } = "One of your heroes has fallen!";

        [SettingPropertyText("Enemy Unassigned Hero Fallen", -1, true, "", Order = 11, RequireRestart = false, HintText = "This text will be shown when enemy's unassigned hero will fall in battle.")]
        [SettingPropertyGroup("On-screen notifications texts", GroupOrder = 3)]
        public string enemyUnassignedHeroFallenNotification { get; set; } = "One of their heroes has fallen!";

        // Technical settings

        [SettingPropertyInteger("Mod update interval in seconds", 0, 100, "0", Order = 1, RequireRestart = false, HintText = "This mod updates hero and troop lists every XX seconds. It doesn’t need to be changed. [Default: 30]")]
        [SettingPropertyGroup("Technical settings", GroupOrder = 4)]
        public int UpdateIntervalInSeconds { get; set; } = 30;

        [SettingPropertyBool("Logging for debugging", Order = 2, RequireRestart = false, HintText = "Logging for debugging. [Default: disabled]")]
        [SettingPropertyGroup("Technical settings", GroupOrder = 4)]
        public bool LoggingEnabled { get; set; } = false;
    }
}