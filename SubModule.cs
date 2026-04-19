using TaleWorlds.MountAndBlade;

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