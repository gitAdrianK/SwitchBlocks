namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.IO;
    using Behaviours;
    using Blocks;
    using Data;
    using Entities;
    using Factories.Drawables;
    using JumpKing;
    using JumpKing.Player;
    using Settings;
    using Util;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupCountdown
    {
        /// <summary>Whether the countdown block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; } = new HashSet<int>();

        /// <summary>Countdown single use lever blocks.</summary>
        public static Dictionary<int, IBlockGroupId> SingleUseLevers { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// ///
        /// <param name="settings">Settings of the countdown type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        public static void Setup(SettingsCountdown settings, BodyComp body)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataCountdown.Instance;

            var seeds = SeedsCountdown.TryDeserialize();
            AssignGroupIds(seeds.Seeds);
            if (LevelDebugState.instance == null)
            {
                SingleUseLevers.Clear();
            }
            else
            {
                seeds.SaveToFile();
            }

            var entityLogic = new EntityLogicCountdown(settings);

            var xmlPath = Path.Combine(ModEntry.ModPath, ModConstants.Countdown);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataCountdown.Instance);
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataCountdown.Instance, entityLogic);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataCountdown.Instance,
                    entityLogic, false);
            }
            else
            {
                FactoryDrawables.CreateDrawablesLegacy(
                    FactoryDrawables.DrawType.Platforms,
                    FactoryDrawables.BlockType.Countdown,
                    entityLogic);
                FactoryDrawables.CreateDrawablesLegacy(
                    FactoryDrawables.DrawType.Levers,
                    FactoryDrawables.BlockType.Countdown,
                    entityLogic);
                FactoryDrawables.CreateDrawablesLegacy(
                    FactoryDrawables.DrawType.Conveyors,
                    FactoryDrawables.BlockType.Countdown,
                    entityLogic);
            }

            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownOn), new BehaviourCountdownOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownOff), new BehaviourCountdownOff());
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownLever),
                new BehaviourCountdownLever(settings.LeverDirections));
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownSingleUse),
                new BehaviourCountdownSingleUse(settings.LeverDirections));
        }

        /// <summary>
        ///     Cleans up saving data, resetting fields and does other required actions.
        /// </summary>
        public static void Cleanup()
        {
            if (!IsUsed)
            {
                return;
            }

            DataCountdown.Instance.SaveToFile();
            DataCountdown.Reset();

            IsUsed = false;
        }

        /// <summary>
        ///     Assigns group IDs to all single use blocks.
        /// </summary>
        /// <param name="seeds">Seeds to use for assignment.</param>
        private static void AssignGroupIds(Dictionary<int, int> seeds)
        {
            var groupId = 1;

            if (seeds.Count != 0)
            {
                BlockGroupId.AssignGroupIdsFromSeed(
                    seeds,
                    ref groupId,
                    SingleUseLevers);
            }

            BlockGroupId.AssignGroupIdsConsecutively(SingleUseLevers, seeds, ref groupId);
        }
    }
}
