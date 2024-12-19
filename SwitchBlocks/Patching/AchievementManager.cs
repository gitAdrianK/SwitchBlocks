namespace SwitchBlocks.Patching
{
    using HarmonyLib;
    using JumpKing.MiscSystems.Achievements;

    public class AchievementManager
    {
        private static readonly Traverse TraverseAM;

        static AchievementManager()
        {
            var achievemementManager = AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:instance");
            TraverseAM = Traverse.Create(achievemementManager.GetValue(null));
        }

        public static int GetTicks()
        {
            var allTimeTicks = TraverseAM
                        .Field("m_all_time_stats")
                        .GetValue<PlayerStats>()
                        ._ticks;
            var snapshotTicks = TraverseAM
                        .Field("m_snapshot")
                        .GetValue<PlayerStats>()
                        ._ticks;
            return allTimeTicks - snapshotTicks;
        }
    }
}
