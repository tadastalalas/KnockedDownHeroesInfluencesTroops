using TaleWorlds.MountAndBlade;

namespace KnockedDownHeroesInfluencesTroops
{
    public class SubModule : MBSubModuleBase
    {
        public override void OnMissionBehaviorInitialize(Mission mission)
        {
            base.OnMissionBehaviorInitialize(mission);

            if (mission == null)
                return;

            if (!mission.IsFieldBattle && !mission.IsSiegeBattle)
                return;

            mission.AddMissionBehavior(new KnockedDownHeroesInfluencesTroopsMissionBehavior());
        }
    }
}