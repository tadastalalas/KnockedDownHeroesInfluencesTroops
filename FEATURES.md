# KnockedDownHeroesInfluencesTroops — Features

| Feature | Gate | File |
|---|---|---|
| Hero knocked down in a field or siege battle: losing side loses morale, attacking side gains morale (general → whole team, captain → its formation, unassigned hero → troops in range) | setting:EnableThisModification (+ 12 penalty and 12 gain settings) | `Missions\KnockedDownHeroesInfluencesTroopsMissionBehavior.cs`, `Missions\MissionUtilities.cs`, `Core\Balance.cs`, `SubModule.cs` |
| Captain and troop lists rebuilt every few seconds | setting:UpdateIntervalInSeconds | `Missions\KnockedDownHeroesInfluencesTroopsMissionBehavior.cs`, `Missions\FormationKind.cs` |
| Attacker's troops cheer, spread over frames | setting:EnableThisModification | `Missions\MissionUtilities.cs`, `Core\Balance.cs` |
| On-screen notification with sound when a hero falls | setting:ShowOnScreenNotifications, HideUnassignedHeroNotifications, DisableKnockdownSounds | `Missions\MissionUtilities.cs` |
| Editable notification texts; defaults shown translated | setting:12 notification texts | `Missions\MissionUtilities.cs`, `Data\ModConfiguration.cs`, `Core\Texts.cs` |
| Player's voice line when a friendly general or captain falls | setting:PlayHeroFallReactionVoiceLines | `Missions\MissionUtilities.cs`, `_Module\ModuleData\module_sounds.xml`, `_Module\ModuleSounds\` |
| Settings: MCM when loaded, otherwise ModConfiguration.xml | always | `Core\ModSettings.cs`, `Data\MCMSettings.cs`, `Data\ModConfiguration.cs`, `_Module\ModConfiguration.xml` |
| Log file (problems always, debug lines only when enabled) | setting:LoggingEnabled | `Diagnostics\KdhitLog.cs`, `Core\ModInfo.cs` |
| Localization (English + 12 languages) | — | `Core\Texts.cs`, `_Module\ModuleData\Languages\` |
