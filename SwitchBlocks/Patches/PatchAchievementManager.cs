namespace SwitchBlocks.Patches
{
    using HarmonyLib;
    using JumpKing.MiscSystems.Achievements;

    /// <summary>
    /// Adds the function GetTick, giving access to the current gametick
    /// from the vanilla AchievementManager.
    /// </summary>
    public class PatchAchievementManager
    {
        /// <summary>The <see cref="Traverse"/> instance of the vanilla AchievementManager.</summary>
        private static readonly Traverse TraverseAM;

        /// <summary>Static ctor. Variable setup.</summary>
        static PatchAchievementManager()
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
