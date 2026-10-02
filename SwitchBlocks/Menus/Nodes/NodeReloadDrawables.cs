namespace SwitchBlocks.Menus
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using BehaviorTree;
    using Data;
    using Entities;
    using EntityComponent;
    using Factories.Drawables;
    using JumpKing;
    using JumpKing.Player;
    using Setups;

    /// <summary>
    ///     A <see cref="IBTnode" /> responsible for reloading the mods drawables.
    /// </summary>
    public class NodeReloadDrawables : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            var directoryBin = new DirectoryInfo(Game1.instance.contentManager.root);
            if (directoryBin.Name != "bin" || directoryBin.Parent == null)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

            var directoryMod = Path.Combine(directoryBin.Parent.FullName, ModConstants.Folder);
            if (!Directory.Exists(directoryMod))
            {
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

            var texturesPath = Path.Combine(directoryBin.FullName, ModConstants.Folder, ModConstants.Textures);
            Debugger.Log(1, "", $"> {texturesPath}\n");

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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataAuto.Instance, entityLogic,
                foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataAuto.Instance, entityLogic,
                foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataAuto.Instance, entityLogic,
                foregroundEntities, midgroundEntities, false);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryLevers.CreateLevers(xmlPath, texturesPath, DataBasic.Instance, foregroundEntities,
                midgroundEntities);
            FactoryLevers.CreateSingleUses(xmlPath, texturesPath, DataBasic.Instance, foregroundEntities,
                midgroundEntities);
            FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataBasic.Instance, entityLogic,
                foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataBasic.Instance,
                entityLogic, foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataBasic.Instance,
                entityLogic, foregroundEntities, midgroundEntities, false);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryLevers.CreateLevers(xmlPath, texturesPath, DataCountdown.Instance, foregroundEntities,
                midgroundEntities);
            FactoryLevers.CreateSingleUses(xmlPath, texturesPath, DataCountdown.Instance, foregroundEntities,
                midgroundEntities);
            FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataCountdown.Instance,
                entityLogic, foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataCountdown.Instance,
                entityLogic, foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataCountdown.Instance,
                entityLogic, foregroundEntities, midgroundEntities, false);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryPlatforms.CreateGroupPlatforms(xmlPath, texturesPath, DataGroup.Instance.Groups,
                entityLogic, foregroundEntities, midgroundEntities);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataJump.Instance, entityLogic,
                foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataJump.Instance, entityLogic,
                foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataJump.Instance,
                entityLogic, foregroundEntities, midgroundEntities, false);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryLevers.CreateLevers(xmlPath, texturesPath, DataSand.Instance, foregroundEntities,
                midgroundEntities);
            FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataSand.Instance,
                entityLogic, foregroundEntities, midgroundEntities, true);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryPlatforms.CreateGroupPlatforms(xmlPath, texturesPath, DataSequence.Instance.Groups,
                entityLogic, foregroundEntities, midgroundEntities);
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
            if (!Directory.Exists(xmlPath))
            {
                return;
            }

            FactoryPlatforms.CreatePlatforms(xmlPath, texturesPath, DataThreshold.Instance,
                entityLogic, foregroundEntities, midgroundEntities);
            FactoryScrolling.CreatePlatformsSand(xmlPath, texturesPath, DataThreshold.Instance,
                entityLogic, foregroundEntities, midgroundEntities);
        }
    }
}
