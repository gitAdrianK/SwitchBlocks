namespace SwitchBlocks.Menus
{
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
    ///     A BtNode responsible for reloading the mods drawables.
    /// </summary>
    public class NodeReloadDrawables : IBTnode
    {
        /// <inheritdoc />
        protected override BTresult MyRun(TickData tickData)
        {
            if (!ModDebug.IsDebug)
            {
                Game1.instance.contentManager.audio.menu.MenuFail.Play();
                return BTresult.Failure;
            }

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
            var entities = entityManager.Entities.ToList();
            foreach (var entity in entities.OfType<EntityDraw>())
            {
                entity.Destroy();
            }

            var texturesPath = Path.Combine(directoryMod, ModConstants.Textures);
            if (SetupAuto.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicAuto;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Auto);
                if (Directory.Exists(xmlPath))
                {
                    FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataAuto.Instance, entityLogic);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataAuto.Instance,
                        entityLogic,
                        false);
                }
                else
                {
                    xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Auto);
                    FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataAuto.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Auto);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataAuto.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Auto);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataAuto.Instance, entityLogic, false, true);
                }
            }

            if (SetupBasic.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicBasic;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Basic);
                if (Directory.Exists(xmlPath))
                {
                    FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataBasic.Instance);
                    FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataBasic.Instance, entityLogic);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataBasic.Instance,
                        entityLogic);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataBasic.Instance,
                        entityLogic, false);
                }
                else
                {
                    // The legacy folder structure is not as unified.
                    xmlPath = Path.Combine(ModEntry.RootModFolder, "levers", ModConstants.Basic);
                    FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataBasic.Instance);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Basic);
                    FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataBasic.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Basic);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataBasic.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Basic);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataBasic.Instance, entityLogic, false, true);
                }
            }

            if (SetupCountdown.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicCountdown;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Countdown);
                if (Directory.Exists(xmlPath))
                {
                    FactoryLevers.CreateLevers(xmlPath, ModEntry.TexturePath, DataCountdown.Instance);
                    FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataCountdown.Instance,
                        entityLogic);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataCountdown.Instance,
                        entityLogic);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataCountdown.Instance,
                        entityLogic, false);
                }
                else
                {
                    xmlPath = Path.Combine(ModEntry.RootModFolder, "levers", ModConstants.Countdown);
                    FactoryLevers.CreateLevers(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataCountdown.Instance);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Countdown);
                    FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataCountdown.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Countdown);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataCountdown.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Countdown);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataCountdown.Instance, entityLogic, false, true);
                }
            }

            if (SetupGroup.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicGroup;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Group);
                if (Directory.Exists(xmlPath))
                {
                    FactoryPlatforms.CreateGroupPlatforms(xmlPath, texturesPath, DataGroup.Instance.Groups,
                        entityLogic);
                }
                else
                {
                    xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Group);
                    FactoryPlatforms.CreateGroupPlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataGroup.Instance.Groups,
                        entityLogic);
                }
            }

            if (SetupJump.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicJump;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Jump);
                if (Directory.Exists(xmlPath))
                {
                    FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataJump.Instance, entityLogic);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataJump.Instance, entityLogic);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, ModEntry.TexturePath, DataJump.Instance,
                        entityLogic, false);
                }
                else
                {
                    xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Jump);
                    FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataJump.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Jump);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataJump.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "conveyors", ModConstants.Jump);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataJump.Instance, entityLogic, false, true);
                }
            }

            if (SetupSand.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicSand;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Sand);
                if (Directory.Exists(xmlPath))
                {
                    FactoryLevers.CreateLevers(xmlPath, texturesPath, DataSand.Instance);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath, texturesPath, DataSand.Instance,
                        entityLogic,
                        true);
                }
                else
                {
                    xmlPath = Path.Combine(directoryMod, "levers", ModConstants.Sand);
                    FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataSand.Instance, entityLogic);

                    xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Sand);
                    FactoryScrolling.CreatePlatformsScrolling(xmlPath,
                        Path.Combine(xmlPath, ModConstants.Textures),
                        DataSand.Instance,
                        entityLogic,
                        true,
                        true);
                }
            }

            if (SetupSequence.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicSequence;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Sequence);
                if (Directory.Exists(xmlPath))
                {
                    FactoryPlatforms.CreateGroupPlatforms(xmlPath, texturesPath, DataSequence.Instance.Groups,
                        entityLogic);
                }
                else
                {
                    xmlPath = Path.Combine(directoryMod, "platforms", ModConstants.Sequence);
                    FactoryPlatforms.CreateGroupPlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataSequence.Instance.Groups,
                        entityLogic);
                }
            }

            if (SetupThreshold.IsUsed)
            {
                var entityLogic = debugInstance.EntityLogicThreshold;
                var xmlPath = Path.Combine(directoryMod, ModConstants.Threshold);
                if (Directory.Exists(xmlPath))
                {
                    FactoryPlatforms.CreatePlatforms(xmlPath, ModEntry.TexturePath, DataThreshold.Instance,
                        entityLogic);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, ModEntry.TexturePath, DataThreshold.Instance,
                        entityLogic);
                }
                else
                {
                    xmlPath = Path.Combine(ModEntry.RootModFolder, "platforms", ModConstants.Threshold);
                    FactoryPlatforms.CreatePlatforms(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataThreshold.Instance, entityLogic);

                    xmlPath = Path.Combine(ModEntry.RootModFolder, "sands", ModConstants.Threshold);
                    FactoryScrolling.CreatePlatformsSand(xmlPath, Path.Combine(xmlPath, ModConstants.Textures),
                        DataThreshold.Instance, entityLogic);
                }
            }

            entities = entityManager.Entities
                .SkipWhile(entity => !(entity is PlayerEntity))
                .ToList();
            foreach (var entity in entities)
            {
                if (!(entity is EntityDraw entityDraw) || entityDraw.IsForeground)
                {
                    entity.GoToFront();
                }
            }

            Game1.instance.contentManager.audio.menu.Select.Play();
            return BTresult.Success;
        }
    }
}
