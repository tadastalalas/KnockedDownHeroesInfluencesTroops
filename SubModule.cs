using KnockedDownHeroesInfluencesTroops.Core;
using KnockedDownHeroesInfluencesTroops.Missions;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
// ReSharper disable UnusedType.Global

namespace KnockedDownHeroesInfluencesTroops;

public class SubModule : MBSubModuleBase
{
    // Any game type (campaign, custom battle): the mod works in every field and siege battle.
    protected override void OnGameStart(Game game, IGameStarter gameStarterObject)
    {
        base.OnGameStart(game, gameStarterObject);
        ModSettings.Reload();
    }

    public override void OnMissionBehaviorInitialize(Mission mission)
    {
        base.OnMissionBehaviorInitialize(mission);
        mission.AddMissionBehavior(new KnockedDownHeroesInfluencesTroopsMissionBehavior());
    }
}
