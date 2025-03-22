namespace SwitchBlocks.Patching
{
    using HarmonyLib;
    using JumpKing.MiscSystems.Achievements;

    /// <summary>
    /// Adds a function for the vanilla AchievementManager.
    /// </summary>
    public class AchievementManager
    {
        /// <summary>The <see cref="Traverse"/> instance of the vanilla AchievementManager.</summary>
        private static readonly Traverse TraverseAM;

        /// <summary>Static ctor. Variable setup.</summary>
        static AchievementManager()
        {
            var achievemementManager = AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:instance");
            TraverseAM = Traverse.Create(achievemementManager.GetValue(null));
        }

        /// <summary>
        /// Get the current tick of the game.
        /// </summary>
        /// <returns>Current game tick.</returns>
        public static int GetTick()
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
