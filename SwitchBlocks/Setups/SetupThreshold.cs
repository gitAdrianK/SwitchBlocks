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

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupThreshold
    {
        /// <summary>Whether the threshold block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the threshold type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void Setup(SettingsThreshold settings, BodyComp body, List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!IsUsed)
            {
                return;
            }

            PatchModLoader.AddDebugMessage("[INFO] Beginning THRESHOLD Setup.");

            PatchModLoader.AddDebugMessage("[INFO] Trying to load from file.");
            _ = DataThreshold.Instance;

            PatchModLoader.AddDebugMessage("[INFO] Creating logic entity.");
            var entityLogic = new EntityLogicThreshold(settings);

            PatchModLoader.AddDebugMessage("[INFO] Creating drawables.");
            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Threshold);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataThreshold.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataThreshold.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
            }
            else
            {
                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Threshold);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataThreshold.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Threshold);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataThreshold.Instance, entityLogic, foregroundEntities, midgroundEntities);
            }

            PatchModLoader.AddDebugMessage("[INFO] Creating behaviours.");
            _ = body.RegisterBlockBehaviour(typeof(BlockThresholdOn), new BehaviourThresholdOn());
            _ = body.RegisterBlockBehaviour(typeof(BlockThresholdOff), new BehaviourThresholdOff());
            var behaviourReset = new BehaviourThresholdReset(settings.Stat);
            _ = body.RegisterBlockBehaviour(typeof(BlockThresholdReset), behaviourReset);

            // ReSharper disable once InvertIf
            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicThreshold = entityLogic;
                debugInstance.BehaviourThresholdReset = behaviourReset;
            }

            PatchModLoader.AddDebugMessage("[INFO] Finished THRESHOLD Setup.");
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

            PatchModLoader.AddDebugMessage("[INFO] Beginning THRESHOLD Cleanup.");

            PatchModLoader.AddDebugMessage("[INFO] Saving to file.");
            DataThreshold.Instance.SaveToFile();
            DataThreshold.Reset();

            IsUsed = false;
            PatchModLoader.AddDebugMessage("[INFO] Finished THRESHOLD Cleanup.");
        }
    }
}
