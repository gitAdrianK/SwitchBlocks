namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.IO;
    using Behaviours;
    using Blocks;
    using Data;
    using Entities;
    using Factories.Drawables;
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

        /// <summary>Countdown custom duration lever blocks.</summary>
        public static Dictionary<int, IBlockDuration> CustomDurationLevers { get; } =
            new Dictionary<int, IBlockDuration>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// ///
        /// <param name="settings">Settings of the countdown type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void Setup(SettingsCountdown settings, BodyComp body, List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!IsUsed)
            {
                return;
            }

            _ = DataCountdown.Instance;

            var seedsId = SeedsCountdown.TryDeserialize();
            AssignByGroups(seedsId.Seeds);

            var seedsDuration = DurationsCountdown.TryDeserialize();
            AssignByDuration(seedsDuration.Seeds);

            if (!ModDebug.IsDebug)
            {
                SingleUseLevers.Clear();
                CustomDurationLevers.Clear();
            }
            else
            {
                seedsId.SaveToFile();
                seedsDuration.SaveToFile();
            }

            var entityLogic = new EntityLogicCountdown(settings);

            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Countdown);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataCountdown.Instance, foregroundEntities,
                    midgroundEntities);
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataCountdown.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataCountdown.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataCountdown.Instance,
                    entityLogic, foregroundEntities, midgroundEntities, false);
            }
            else
            {
                xmlPath = Path.Combine(ModEntry.RootModFolder, "levers", ModConstants.Countdown);
                FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Countdown);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Countdown);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Countdown);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }

            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownOn), new BehaviourCountdownOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownOff), new BehaviourCountdownOff());
            var behaviourLever = new BehaviourCountdownLever(settings.LeverDirections, settings.Duration);
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownLever), behaviourLever);
            var behaviourLeverSingleUse = new BehaviourCountdownSingleUse(settings.LeverDirections);
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownSingleUse), behaviourLeverSingleUse);
            var behaviourLeverCustomDuration = new BehaviourCountdownCustomDuration(settings.LeverDirections);
            _ = body.RegisterBlockBehaviour(typeof(BlockCountdownCustomDuration), behaviourLeverCustomDuration);

            // ReSharper disable once InvertIf
            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicCountdown = entityLogic;
                debugInstance.BehaviourCountdownLever = behaviourLever;
                debugInstance.BehaviourCountdownSingleUse = behaviourLeverSingleUse;
                debugInstance.BehaviourCountdownCustomDuration = behaviourLeverCustomDuration;
            }
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
        public static void AssignByGroups(Dictionary<int, int> seeds)
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

        /// <summary>
        ///     Assigns durations to all custom duration blocks.
        /// </summary>
        /// <param name="seeds">Seeds to use for assignment.</param>
        public static void AssignByDuration(Dictionary<int, float> seeds)
        {
            if (seeds.Count != 0)
            {
                BlockDuration.AssignDurationsFromSeed(
                    seeds,
                    CustomDurationLevers);
            }

            BlockDuration.AssignOtherDurations(CustomDurationLevers, seeds);
        }
    }
}
