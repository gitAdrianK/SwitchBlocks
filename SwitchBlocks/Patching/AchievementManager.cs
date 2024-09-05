using HarmonyLib;
using System.Reflection;

namespace SwitchBlocks.Patching
{
    public class AchievementManager
    {
        public static FieldInfo AchievemementManager;

        static AchievementManager()
        {
            AchievemementManager = AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:instance");
        }

        public static int GetTicks()
        {
            int allTimeTicks = Traverse.Create(AchievemementManager.GetValue(null))
                        .Field("m_all_time_stats")
                        .Field("_ticks")
                        .GetValue<int>();
            int snapshotTicks = Traverse.Create(AchievemementManager.GetValue(null))
                        .Field("m_snapshot")
                        .Field("_ticks")
                        .GetValue<int>();
            return allTimeTicks - snapshotTicks;
        }
    }
}
