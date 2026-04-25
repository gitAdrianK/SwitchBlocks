namespace SwitchBlocks.Setups
{
    using System.Collections.Generic;
    using System.IO;
    using Behaviours;
    using Blocks;
    using Data;
    using Entities;
    using Factories.Drawables;
    using JumpKing.API;
    using JumpKing.Player;
    using Patches;
    using Settings;

    /// <summary>
    ///     Setup and cleanup as well as setup related fields.
    /// </summary>
    public static class SetupSand
    {
        /// <summary>Whether the sand block appears inside the hitbox file and counts as used.</summary>
        public static bool IsUsed { get; set; }

        /// <summary>
        ///     Sets up data, entities, block behaviours and does other required actions.
        /// </summary>
        /// <param name="settings">Settings of the sand type.</param>
        /// <param name="body"><see cref="BodyComp" /> to register block behaviours to.</param>
        /// <param name="collisionQuery">An implementor of <see cref="ICollisionQuery" /></param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void Setup(SettingsSand settings, BodyComp body, ICollisionQuery collisionQuery,
            List<EntityDraw> foregroundEntities, List<EntityDraw> midgroundEntities)
        {
            if (!IsUsed)
            {
                return;
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Beginning SAND Setup.");

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Attempting to load from file.");
            _ = DataSand.Instance;

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating logic entity.");
            var entityLogic = new EntityLogicSand(settings);

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating drawables.");
            var xmlPath = Path.Combine(ModEntry.RootModFolder, ModConstants.Sand);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataSand.Instance, foregroundEntities,
                    midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataSand.Instance, entityLogic,
                    foregroundEntities, midgroundEntities, true);
            }
            else
            {
                xmlPath = Path.Combine(ModEntry.RootModFolder, "levers", ModConstants.Sand);
                FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataSand.Instance, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Sand);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath,
                    Path.Combine(xmlPath, ModConstants.Textures), DataSand.Instance, entityLogic, foregroundEntities,
                    midgroundEntities, true, true);
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Creating behaviours.");
            if (settings.IsV2)
            {
                // To keep legacy and GotIB without change the new behaviour is behind a v2 setting.
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOn), new BehaviourSandOn(collisionQuery));
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOff), new BehaviourSandOff(collisionQuery));
            }
            else
            {
                // XXX: Do not register the same behaviour for multiple blocks if the behaviour changes
                // velocity or position! This technically needs updating, but I have to consider
                // Ghost of the Immortal Babe breaking!
                var behaviourSandPlatform = new BehaviourSandLegacy();
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOn), behaviourSandPlatform);
                _ = body.RegisterBlockBehaviour(typeof(BlockSandOff), behaviourSandPlatform);
            }

            var behaviourLever = new BehaviourSandLever(settings.LeverDirections);
            _ = body.RegisterBlockBehaviour(typeof(BlockSandLever), behaviourLever);

            // ReSharper disable once InvertIf
            if (ModDebug.IsDebug)
            {
                var debugInstance = ModDebug.Instance;
                debugInstance.EntityLogicSand = entityLogic;
                debugInstance.BehaviourSandLever = behaviourLever;
            }

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished SAND Setup.\n");
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

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Beginning SAND Cleanup.");

            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Saving to file.");
            DataSand.Instance.SaveToFile();
            DataSand.Reset();

            IsUsed = false;
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished SAND Cleanup.\n");
        }
    }
}
