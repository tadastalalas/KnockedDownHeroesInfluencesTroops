using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public class SubModule : MBSubModuleBase
    {
        protected override void OnSubModuleLoad() => base.OnSubModuleLoad();

        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);
            mission.AddMissionBehavior(new KnockedDownHeroesInfluencesTroopsMissionBehavior());
        }
    }
}