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
    using Patches;
    using Settings;
    using Util;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupBasic
    {
        /// <summary>Whether the basic block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; } = new HashSet<int>();

        /// <summary>Basic single use lever blocks.</summary>
        public static Dictionary<int, IBlockGroupId> SingleUseLevers { get; } = new Dictionary<int, IBlockGroupId>();

        /// <summary>Basic Reset blocks.</summary>
        public static Dictionary<int, IMultipleGroupIds> Resets { get; } = new Dictionary<int, IMultipleGroupIds>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="behaviourPre"><see cref="BehaviourPre" /> to give data to.</param>
        /// <param name="settings">Settings of the basic type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void Setup(BehaviourPre behaviourPre, SettingsBasic settings, BodyComp body,
            List<EntityDraw> foregroundEntities, List<EntityDraw> midgroundEntities)
        {
            if (!IsUsed)
            {
                return;
            }

            PatchControllerManager.CanSwitchOnPress = settings.CanSwitchOnPress;

            var data = DataBasic.Initialize(settings.SaveCarriesOver);
            behaviourPre.Basic = data;

            var seedsId = SeedsBasic.TryDeserialize();
            var resets = ResetsBasic.TryDeserialize();
            AssignByGroups(seedsId.Seeds, resets.Resets);

            var entityLogic = new EntityLogicBasic(settings);

            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Basic);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataBasic.Instance, foregroundEntities,
                    midgroundEntities);
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataBasic.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataBasic.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataBasic.Instance,
                    entityLogic, foregroundEntities, midgroundEntities, false);
            }
            else
            {
                // The legacy folder structure is not as unified.
                xmlPath = Path.Combine(ModEntry.RootModFolder, "levers", ModConstants.Basic);
                FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Basic);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Basic);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Basic);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }

            _ = body.RegisterBlockBehaviour(typeof(BlockBasicOn), new BehaviourBasicOn());

            _ = body.RegisterBlockBehaviour(typeof(BlockBasicOff), new BehaviourBasicOff());

            var behaviourLever = new BehaviourBasicLever(settings.LeverDirections);
            _ = body.RegisterBlockBehaviour(typeof(BlockBasicLever), behaviourLever);

            if (SingleUseLevers.Count != 0)
            {
                var behaviourSingleUse = new BehaviourBasicSingleUse(settings.LeverDirections);
                _ = body.RegisterBlockBehaviour(typeof(BlockBasicSingleUse), behaviourSingleUse);
                if (ModDebug.IsDebug)
                {
                    var debugInstance = ModDebug.Instance;
                    debugInstance.BehaviourBasicSingleUse = behaviourSingleUse;
                }
            }

            if (Resets.Count != 0)
            {
                var behaviourReset = new BehaviourBasicReset(settings.LeverDirections);
                _ = body.RegisterBlockBehaviour(typeof(BlockBasicReset), behaviourReset);
                if (ModDebug.IsDebug)
                {
                    var debugInstance = ModDebug.Instance;
                    debugInstance.BehaviourBasicReset = behaviourReset;
                }
            }

            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicBasic = entityLogic;
                debugInstance.BehaviourBasicLever = behaviourLever;

                seedsId.SaveToFile();
                resets.SaveToFile();
            }
            else
            {
                SingleUseLevers.Clear();
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

            PatchControllerManager.CanSwitchOnPress = false;

            DataBasic.Instance.SaveToFile();
            DataBasic.Reset();

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
    }
}
