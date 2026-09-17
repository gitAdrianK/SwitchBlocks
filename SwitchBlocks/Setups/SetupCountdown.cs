namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.IO;
    using Behaviours;
    using Behaviours.Dummy;
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

        /// <summary>Countdown Reset blocks.</summary>
        public static Dictionary<int, IMultipleGroupIds> Resets { get; } = new Dictionary<int, IMultipleGroupIds>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="behaviourPre"><see cref="BehaviourPre" /> to give data to.</param>
        /// <param name="settings">Settings of the countdown type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void Setup(BehaviourPre behaviourPre, SettingsCountdown settings, BodyComp body,
            List<EntityDraw> foregroundEntities, List<EntityDraw> midgroundEntities)
        {
            if (!IsUsed)
            {
                return;
            }

            var data = DataCountdown.Instance;
            behaviourPre.Countdown = data;

            var seedsId = SeedsCountdown.TryDeserialize();
            var resets = ResetsCountdown.TryDeserialize();
            AssignByGroups(seedsId.Seeds, resets.Resets);

            var seedsDuration = DurationsCountdown.TryDeserialize();
            AssignByDuration(seedsDuration.Seeds);

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

            if (SingleUseLevers.Count != 0)
            {
                var behaviourLeverSingleUse =
                    new BehaviourCountdownSingleUse(settings.LeverDirections, settings.Duration);
                _ = body.RegisterBlockBehaviour(typeof(BlockCountdownSingleUse), behaviourLeverSingleUse);
                if (ModDebug.IsDebug)
                {
                    var debugInstance = ModDebug.Instance;
                    debugInstance.BehaviourCountdownSingleUse = behaviourLeverSingleUse;
                }
            }

            if (CustomDurationLevers.Count != 0)
            {
                var behaviourLeverCustomDuration = new BehaviourCountdownCustomDuration(settings.LeverDirections);
                _ = body.RegisterBlockBehaviour(typeof(BlockCountdownCustomDuration), behaviourLeverCustomDuration);
                if (ModDebug.IsDebug)
                {
                    var debugInstance = ModDebug.Instance;
                    debugInstance.BehaviourCountdownCustomDuration = behaviourLeverCustomDuration;
                }
            }

            if (Resets.Count != 0)
            {
                var behaviourReset = new BehaviourCountdownReset(settings.LeverDirections);
                _ = body.RegisterBlockBehaviour(typeof(BlockCountdownReset), behaviourReset);
                if (ModDebug.IsDebug)
                {
                    var debugInstance = ModDebug.Instance;
                    debugInstance.BehaviourCountdownReset = behaviourReset;
                }
            }

            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicCountdown = entityLogic;
                debugInstance.BehaviourCountdownLever = behaviourLever;

                seedsId.SaveToFile();
                seedsDuration.SaveToFile();
                resets.SaveToFile();
            }
            else
            {
                SingleUseLevers.Clear();
                CustomDurationLevers.Clear();
                Resets.Clear();
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
        /// <param name="resets">Positions to add reset IDs to reset blocks to.</param>
        public static void AssignByGroups(Dictionary<int, int> seeds, Dictionary<int, int[]> resets)
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

            if (resets.Count != 0)
            {
                MultipleGroupIds.AssignMultipleIdsFromSeed(Resets, resets);
            }

            MultipleGroupIds.AssignOtherMultipleIds(Resets, resets);
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
