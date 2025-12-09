namespace SwitchBlocks.Patches
{
    using HarmonyLib;
    using JumpKing.MiscSystems.Achievements;

    /// <summary>
    ///     Adds the function GetTick, giving access to the current game-tick
    ///     from the vanilla AchievementManager. This is technically not a patch
    ///     but a utility function, so one could argue it goes into the
    ///     namespace util, but as it is making extensive use of Harmony tools
    ///     putting it into the patches namespace isn't too far-fetched.
    /// </summary>
    public static class PatchAchievementManager
    {
        /// <summary>The achievement manager instance.</summary>
        private static readonly object AchievementManager =
            AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:instance").GetValue(null);

        /// <summary>FieldRef of the "all-time stats" field.</summary>
        private static readonly AccessTools.FieldRef<object, PlayerStats> AllTimeStatsRef =
            AccessTools.FieldRefAccess<object, PlayerStats>(
                AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:m_all_time_stats"));

        /// <summary>FieldRef of the "snapshot" field.</summary>
        private static readonly AccessTools.FieldRef<object, PlayerStats> SnapshotRef =
            AccessTools.FieldRefAccess<object, PlayerStats>(
                AccessTools.Field("JumpKing.MiscSystems.Achievements.AchievementManager:m_snapshot"));

        /// <summary>
        ///     Get the current tick of the game.
        /// </summary>
        /// <returns>Current game tick.</returns>
        public static int GetTick() =>
            AllTimeStatsRef(AchievementManager)._ticks - SnapshotRef(AchievementManager)._ticks;
    }
}
