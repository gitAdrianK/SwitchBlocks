using HarmonyLib;

namespace SwitchBlocks.Patching
{
    public class AchievementManager
    {
        private static readonly Traverse AllTimeTicks;
        private static readonly Traverse SnapshotTicks;

        static AchievementManager()
        {
            object achievemementManager = AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:instance").GetValue(null);
            AllTimeTicks = Traverse.Create(achievemementManager)
                        .Field("m_all_time_stats")
                        .Field("_ticks");
            SnapshotTicks = Traverse.Create(achievemementManager)
                        .Field("m_snapshot")
                        .Field("_ticks");
        }

        public static int GetTicks()
        {
            return AllTimeTicks.GetValue<int>() - SnapshotTicks.GetValue<int>();
        }
    }
}
