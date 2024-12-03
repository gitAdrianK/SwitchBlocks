using HarmonyLib;
using JumpKing.MiscSystems.Achievements;
using System.Reflection;

namespace SwitchBlocks.Patching
{
    public class AchievementManager
    {
        private static readonly Traverse traverseAM;

        static AchievementManager()
        {
            FieldInfo achievemementManager = AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:instance");
            traverseAM = Traverse.Create(achievemementManager.GetValue(null));
        }

        public static int GetTicks()
        {
            int allTimeTicks = traverseAM
                        .Field("m_all_time_stats")
                        .GetValue<PlayerStats>()
                        ._ticks;
            int snapshotTicks = traverseAM
                        .Field("m_snapshot")
                        .GetValue<PlayerStats>()
                        ._ticks;
            return allTimeTicks - snapshotTicks;
        }
    }
}
