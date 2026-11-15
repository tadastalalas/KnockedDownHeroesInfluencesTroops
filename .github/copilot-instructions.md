KnockedDownHeroesInfluencesTroops: workspace instructions. They override the global file.

- <Prefix>: KDHIT (Kdhit in type names). <Root> namespace and ModuleId: KnockedDownHeroesInfluencesTroops.
- Logger: `Diagnostics\KdhitLog.cs` (Debug / Warn / Error).
- No Harmony patches: no Patches\ folder, no PatchAll, no Lib.Harmony package, no Harmony entries in SubModule.xml.
- `Core\ModInfo.cs` holds the ModuleId, file and folder names and the module root path. Gameplay numbers: `Core\Balance.cs`.
- `ModSettings.Reload()` runs in `SubModule.OnGameStart` for every game type (campaign and custom battle).
  `SubModule.OnMissionBehaviorInitialize` adds the mission behavior; it acts only in field and siege battles.
- The mod saves nothing (no campaign behavior, no SyncData).
- Notification texts: a setting value equal to its English default in `Data\ModConfiguration.cs` is shown as the translated
  `Texts.X`; any other value is shown as typed. `Texts.Ids` builds those 12 ids from the ModConfiguration defaults.
- Kept exception: the MCM Id stays "KnockedDownHeroesInfluencesTroopsSettings" with FormatType "json2". It is released; changing it resets players' settings.
- Kept exception: MCM property names stay as released (incl. lowercase `friendlyGeneralFallenNotification` etc.); they are MCM save keys.
- Kept exception: existing text ids have 7 random characters after KDHIT_. Never rename them.
- Kept exception: sound event ids `kdhit_*` in `_Module\ModuleData\module_sounds.xml` stay lowercase (released).