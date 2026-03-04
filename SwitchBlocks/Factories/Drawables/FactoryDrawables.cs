namespace SwitchBlocks.Factories.Drawables
{
    using System;
    using System.IO;
    using Data;
    using Entities;
    using EntityComponent;
    using JumpKing;

    // LINQ is hard to maintain and read, but it is significantly faster than XmlSerializer, unfortunately.

    // This Factory is legacy and will only be kept around until its functionality is brought to the new factories.

    /// <summary>
    ///     Factory for drawable entities.
    /// </summary>
    public static class FactoryDrawables
    {
        /// <summary>Block types.</summary>
        public enum BlockType
        {
            Auto,
            Basic,
            Countdown,
            Jump,
            Sand,
        }

        /// <summary>Draw types.</summary>
        public enum DrawType
        {
            Platforms,
            Conveyors,
            Levers,
        }

        /// <summary>
        ///     Creates all drawables for a given <see cref="DrawType" /> and <see cref="BlockType" />.
        ///     Created entities are added to the <see cref="EntityManager" /> automatically.
        /// </summary>
        /// <typeparam name="T">A class implementing <see cref="IDataProvider" />.</typeparam>
        /// <param name="drawType"><see cref="DrawType" />.</param>
        /// <param name="blockType"><see cref="BlockType" />.</param>
        /// <param name="entityLogic">
        ///     <see cref="EntityLogic{T}" />
        /// </param>
        /// <exception cref="NotImplementedException">This should never happen.</exception>
        public static void CreateDrawablesLegacy<T>(DrawType drawType, BlockType blockType, EntityLogic<T> entityLogic)
            where T : class, IDataProvider
        {
            var contentManager = Game1.instance.contentManager;

            var path = Path.Combine(
                contentManager.root,
                ModConstants.Folder,
                drawType.ToString(),
                blockType.ToString());
            if (!Directory.Exists(path))
            {
                return;
            }

            var files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return;
            }

            var data = GetData(blockType);
            switch (drawType)
            {
                case DrawType.Platforms:
                    switch (blockType)
                    {
                        case BlockType.Auto:
                        case BlockType.Basic:
                        case BlockType.Countdown:
                        case BlockType.Jump:
                            FactoryPlatforms.CreatePlatformsLegacy(path, files, data, entityLogic);
                            break;
                        case BlockType.Sand:
                            FactoryScrolling.CreatePlatformsScrollingLegacy(path, files, data, entityLogic, true);
                            break;
                        default:
                            throw new NotImplementedException("Unknown Block Type, cannot create entities!");
                    }

                    break;
                case DrawType.Levers:
                    FactoryLevers.CreateLeversLegacy(path, files, data);
                    break;
                case DrawType.Conveyors:
                    FactoryScrolling.CreatePlatformsScrollingLegacy(path, files, data, entityLogic, false);
                    break;
                default:
                    throw new NotImplementedException("Unknown Draw Type, cannot create entities!");
            }
        }

        /// <summary>
        ///     Get the <see cref="IDataProvider" /> based on the <see cref="BlockType" />.
        /// </summary>
        /// <param name="blockType"><see cref="BlockType" />.</param>
        /// <returns><see cref="IDataProvider" />.</returns>
        /// <exception cref="NotImplementedException">This should never happen.</exception>
        private static IDataProvider GetData(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Auto:
                    return DataAuto.Instance;
                case BlockType.Basic:
                    return DataBasic.Instance;
                case BlockType.Countdown:
                    return DataCountdown.Instance;
                case BlockType.Jump:
                    return DataJump.Instance;
                case BlockType.Sand:
                    return DataSand.Instance;
                default:
                    throw new NotImplementedException("Unknown Block Type, cannot get data!");
            }
        }
    }
}
