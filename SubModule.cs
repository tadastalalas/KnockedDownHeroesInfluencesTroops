using TaleWorlds.MountAndBlade;
// ReSharper disable UnusedType.Global

namespace KnockedDownHeroesInfluencesTroops
{
    public class SubModule : MBSubModuleBase
    {
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new KnockedDownHeroesInfluencesTroopsMissionBehavior());
        }
    }
}