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
    using Patches;
    using Settings;
    using Util;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupAuto
    {
        /// <summary>Whether the auto block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        // We trust that map-makers never place different change blocks next to each other.
        /// <summary>Change duration blocks.</summary>
        public static Dictionary<int, IBlockDuration> ChangeDuration { get; } =
            new Dictionary<int, IBlockDuration>();

        /// <summary>Screens that contain a wind enable block.</summary>
        public static HashSet<int> WindEnabled { get; } = new HashSet<int>();

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the auto type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void Setup(SettingsAuto settings, BodyComp body, List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!IsUsed)
            {
                return;
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Beginning AUTO Setup.");

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Attempting to load from file.");
            var data = DataAuto.Instance;

            var seedsDuration = DurationsAuto.TryDeserialize();
            AssignByDuration(seedsDuration.Seeds);

            if (!ModDebug.IsDebug)
            {
                ChangeDuration.Clear();
            }
            else
            {
                seedsDuration.SaveToFile();
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating logic entity.");
            var entityLogic = new EntityLogicAuto(settings);
            if (data.DurationOn > 0 || data.DurationOff > 0)
            {
                var durationOn = data.DurationOn > 0 ? data.DurationOn : entityLogic.DurationOn;
                var durationOff = data.DurationOff > 0 ? data.DurationOff : entityLogic.DurationOff;
                entityLogic.UpdateDurations(durationOn, durationOff);
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating drawables.");
            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Auto);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic,
                    foregroundEntities, midgroundEntities, false);
            }
            else
            {
                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Auto);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Auto);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Auto);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating behaviours.");
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoOn), new BehaviourAutoOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoOff), new BehaviourAutoOff());
            var behaviourReset = new BehaviourAutoReset(settings.DurationOff);
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoReset), behaviourReset);
            _ = body.RegisterBlockBehaviour(typeof(BlockAutoChangeDuration),
                new BehaviourAutoChangeDuration(entityLogic));

            // ReSharper disable once InvertIf
            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicAuto = entityLogic;
                debugInstance.BehaviourAutoReset = behaviourReset;
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished AUTO Setup.\n");
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

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Beginning AUTO Cleanup.");

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Saving to file.");
            DataAuto.Instance.SaveToFile();
            DataAuto.Reset();

            IsUsed = false;
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished AUTO Cleanup.\n");
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
                    ChangeDuration);
            }

            BlockDuration.AssignOtherDurations(ChangeDuration, seeds);
        }
    }
}
