namespace SwitchBlocks.Menus
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using BehaviorTree;
    using Data;
    using Entities;
    using EntityComponent;
    using Factories.Drawables;
    using JumpKing;
    using JumpKing.Player;
    using Patches;
    using Setups;

    /// <summary>
    ///     A BtNode responsible for reloading the mods drawables.
    /// </summary>
    public class NodeReloadDrawables : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Reloading drawables.");

            if (!ModDebug.IsDebug)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                PatchModLoader.AddDebugMessage(
                    $"[WARNING - Switch Blocks] The path '{directoryBin}' did not follow Worldsmith structure.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            if (!Directory.Exists(directoryMod))
            {
                PatchModLoader.AddDebugMessage($"[WARNING - Switch Blocks] The path '{directoryMod}' did not exist.");
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var debugInstance = ModDebug.Instance;
            var entityManager = EntityManager.instance;
            foreach (var entity in entityManager.Entities.ToList().OfType<EntityDraw>())
            {
                entity.Destroy();
            }

            var midgroundEntities = new List<EntityDraw>();
            var foregroundEntities = new List<EntityDraw>();

            var texturesPath = Path.Combine(directoryMod, ModConstants.Textures);

            this.ReloadAutoDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities, midgroundEntities);
            this.ReloadBasicDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities, midgroundEntities);
            this.ReloadCountdownDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities,
                midgroundEntities);
            this.ReloadGroupDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities, midgroundEntities);
            this.ReloadJumpDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities, midgroundEntities);
            this.ReloadSandDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities, midgroundEntities);
            this.ReloadSequenceDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities,
                midgroundEntities);
            this.ReloadThresholdDrawables(debugInstance, directoryMod, texturesPath, foregroundEntities,
                midgroundEntities);

            var entities = entityManager.Entities.ToList();
            foreach (var entity in entities.Where(entity => !(entity is EntityDraw)))
            {
                if (entity is PlayerEntity)
                {
                    foreach (var midgroundEntity in midgroundEntities)
                    {
                        midgroundEntity.GoToFront();
                    }
                }

                entity.GoToFront();
            }

            foreach (var foregroundEntity in foregroundEntities)
            {
                foregroundEntity.GoToFront();
            }


            PatchModLoader.AddDebugMessage("[INFO - Switch Blocks] Finished reloading drawables.");
            PatchModLoader.WriteDebugLoadLog();
            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }

        /// <summary>
        ///     Reload all drawable entities related to the auto block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadAutoDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupAuto.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicAuto;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Auto);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataAuto.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataAuto.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataAuto.Instance, entityLogic,
                    foregroundEntities, midgroundEntities, false);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Auto);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "sands", ModConstants.Auto);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "conveyors", ModConstants.Auto);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataAuto.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the basic block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadBasicDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupBasic.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicBasic;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Basic);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, texturesPath, DataBasic.Instance, foregroundEntities,
                    midgroundEntities);
                FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataBasic.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataBasic.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataBasic.Instance,
                    entityLogic, foregroundEntities, midgroundEntities, false);
            }
            else
            {
                // The legacy folder structure is not as unified.
                xmlPath = Path.Combine(directoryMod, "levers", ModConstants.Basic);
                FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Basic);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "sands", ModConstants.Basic);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "conveyors", ModConstants.Basic);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataBasic.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the countdown block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadCountdownDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupCountdown.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicCountdown;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Countdown);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, texturesPath, DataCountdown.Instance, foregroundEntities,
                    midgroundEntities);
                FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataCountdown.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataCountdown.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataCountdown.Instance,
                    entityLogic, foregroundEntities, midgroundEntities, false);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "levers", ModConstants.Countdown);
                FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Countdown);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "sands", ModConstants.Countdown);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "conveyors", ModConstants.Countdown);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataCountdown.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the group block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadGroupDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupGroup.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicGroup;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Group);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreateGroupPlatforms(xmlPath, texturesPath, DataGroup.Instance.Groups,
                    entityLogic, foregroundEntities, midgroundEntities);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Group);
                FactoryPlatforms.CreateGroupPlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataGroup.Instance.Groups,
                    entityLogic, foregroundEntities, midgroundEntities);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the jump block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadJumpDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupJump.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicJump;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Jump);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataJump.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataJump.Instance, entityLogic,
                    foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataJump.Instance,
                    entityLogic, foregroundEntities, midgroundEntities, false);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Jump);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataJump.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "sands", ModConstants.Jump);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataJump.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "conveyors", ModConstants.Jump);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataJump.Instance, entityLogic, foregroundEntities, midgroundEntities, false, true);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the sand block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadSandDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupSand.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicSand;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Sand);
            if (Directory.Exists(xmlPath))
            {
                FactoryLevers.CreateLevers(xmlPath, texturesPath, DataSand.Instance, foregroundEntities,
                    midgroundEntities);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataSand.Instance,
                    entityLogic, foregroundEntities, midgroundEntities, true);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "levers", ModConstants.Sand);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataSand.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Sand);
                FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataSand.Instance, entityLogic, foregroundEntities, midgroundEntities, true, true);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the sequence block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadSequenceDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupSequence.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicSequence;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Sequence);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreateGroupPlatforms(xmlPath, texturesPath, DataSequence.Instance.Groups,
                    entityLogic, foregroundEntities, midgroundEntities);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Sequence);
                FactoryPlatforms.CreateGroupPlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataSequence.Instance.Groups,
                    entityLogic, foregroundEntities, midgroundEntities);
            }
        }

        /// <summary>
        ///     Reload all drawable entities related to the threshold block type.
        /// </summary>
        /// <param name="debugInstance">Debug instance holding the logic entity.</param>
        /// <param name="directoryMod">Directory of the mod folder.</param>
        /// <param name="texturesPath">Path of the texture folder.</param>
        /// <param name="foregroundEntities">List to add entities to that are later moved to the foreground.</param>
        /// <param name="midgroundEntities">List of entities that will be sorted to the very background.</param>
        private void ReloadThresholdDrawables(ModDebug debugInstance,
            string directoryMod,
            string texturesPath,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!SetupThreshold.IsUsed)
            {
                return;
            }

            var entityLogic = debugInstance.EntityLogicThreshold;
            var xmlPath = Path.Combine(directoryMod, ModConstants.Threshold);
            if (Directory.Exists(xmlPath))
            {
                FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataThreshold.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
                FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataThreshold.Instance,
                    entityLogic, foregroundEntities, midgroundEntities);
            }
            else
            {
                xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Threshold);
                FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataThreshold.Instance, entityLogic, foregroundEntities, midgroundEntities);

                xmlPath = Path.Combine(directoryMod, "sands", ModConstants.Threshold);
                FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                    DataThreshold.Instance, entityLogic, foregroundEntities, midgroundEntities);
            }
        }
    }
}
