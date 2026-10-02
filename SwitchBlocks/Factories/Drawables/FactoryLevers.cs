namespace SwitchBlocks.Factories.Drawables
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Data;
    using Entities;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Setups;
    using Util;
    using Util.Deserialization;

    public static class FactoryLevers
    {
        // TODO: Looking at this again, this (drawable factories) also needs some major clean-up,
        //  going to see about adding it to the over-engineering branch.
        //  (which isn't over-engineering at all, but as long as I don't sit down and test everything properly I'll name it that way)

        /// <summary>The regex for lever files.</summary>
        private static Regex RegexLevers { get; } = new Regex(@"^levers(\d+).xml$");

        /// <summary>The regex for singleUse files.</summary>
        private static Regex RegexSingleUses { get; } = new Regex(@"^singleUses(\d+).xml$");

        /// <summary>
        ///     Creates <see cref="EntityDrawLever" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="data">Data for the entity.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        public static void CreateLevers(
            string xmlPath,
            string texturePath,
            IDataProvider data,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
        {
            if (!Directory.Exists(xmlPath) || !Directory.Exists(texturePath))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(xmlPath))
            {
                var match = RegexLevers.Match(Path.GetFileName(file));
                if (!match.Success || !int.TryParse(match.Groups[1].Value, out var screenIndex))
                {
                    continue;
                }

                var screen = screenIndex - 1;
                if (screen < 0)
                {
                    continue;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root?.Name != "Levers")
                    {
                        return;
                    }

                    foreach (var leverElement in root.Elements("Lever"))
                    {
                        var lever = TryParseLeverElement(leverElement, texturePath);
                        if (lever == null)
                        {
                            continue;
                        }

                        var entity = new EntityDrawLever(lever, screen, data);
                        if (lever.IsForeground)
                        {
                            foregroundEntities.Add(entity);
                        }
                        else if (!lever.IsBackground)
                        {
                            midgroundEntities.Add(entity);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Creates <see cref="EntityDrawSingleUse" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="data">Data for the entity.</param>
        /// <param name="foregroundEntities">Entities that are supposed to be moved into the foreground.</param>
        /// <param name="midgroundEntities">Entities that are supposed to be moved into the midground.</param>
        /// <typeparam name="T">A class implementing <see cref="IGroupDataProvider" /> and <see cref="ITouchedProvider" />.</typeparam>
        public static void CreateSingleUses<T>(string xmlPath,
            string texturePath,
            T data,
            List<EntityDraw> foregroundEntities,
            List<EntityDraw> midgroundEntities)
            where T : IDataProvider, ITouchedProvider
        {
            if (!Directory.Exists(xmlPath) || !Directory.Exists(texturePath))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(xmlPath))
            {
                var match = RegexSingleUses.Match(Path.GetFileName(file));
                if (!match.Success || !int.TryParse(match.Groups[1].Value, out var screenIndex))
                {
                    continue;
                }

                var screen = screenIndex - 1;
                if (screen < 0)
                {
                    continue;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root?.Name != "SingleUses")
                    {
                        Debugger.Log(1, "", ">A\n");
                        return;
                    }

                    foreach (var singleUseElement in root.Elements("SingleUse"))
                    {
                        var lever = TryParseLeverElement(singleUseElement, texturePath);
                        if (lever == null)
                        {
                            Debugger.Log(1, "", ">B\n");
                            continue;
                        }

                        var groupId = FindGroupId<T>(singleUseElement, screen, lever.Position);
                        if (groupId == 0)
                        {
                            Debugger.Log(1, "", ">C\n");
                            continue;
                        }

                        var entity = new EntityDrawSingleUse(lever, screen, data, data.Touched, groupId);
                        if (lever.IsForeground)
                        {
                            foregroundEntities.Add(entity);
                        }
                        else if (!lever.IsBackground)
                        {
                            midgroundEntities.Add(entity);
                        }
                    }
                }
            }
        }

        /// <summary>
        ///     Helper to try to parse an <see cref="XElement" /> to a lever.
        /// </summary>
        /// <param name="leverElement">The element to create a lever from.</param>
        /// <param name="texturePath">Path to the texture.</param>
        /// <returns>A lever if creation was successful, <c>null</c> otherwise.</returns>
        private static Lever TryParseLeverElement(XElement leverElement, string texturePath)
        {
            if (leverElement == null)
            {
                Debugger.Log(1, "", ">a\n");
                return null;
            }

            var textureElement = leverElement.Element("Texture");
            var positionElement = leverElement.Element("Position");

            if (textureElement == null || positionElement == null)
            {
                Debugger.Log(1, "", ">b\n");
                return null;
            }

            var textureFile = Path.Combine(texturePath, textureElement.Value);
            if (!File.Exists(textureFile + ".xnb"))
            {
                Debugger.Log(1, "", $">c {textureFile}\n");
                return null;
            }

            if (!TryParseVector2(positionElement, out var position))
            {
                Debugger.Log(1, "", ">d\n");
                return null;
            }

            return new Lever
            {
                Texture = Game1.instance.contentManager.Load<Texture2D>(textureFile),
                Position = position,
                IsForeground = XmlHelper.ParseElementBool(leverElement, "IsForeground"),
                IsBackground = XmlHelper.ParseElementBool(leverElement, "IsBackground"),
            };
        }

        /// <summary>
        ///     Helper to find, for a block type, the group id.
        /// </summary>
        /// <param name="platformElement">The platform XElement.</param>
        /// <param name="screen">The screen.</param>
        /// <param name="position">The position.</param>
        /// <returns>The found group id or 0 otherwise.</returns>
        /// <exception cref="NotSupportedException">Should the given type T not be supported.</exception>
        private static int FindGroupId<T>(
            XElement platformElement,
            int screen,
            Vector2 position)
            where T : IDataProvider
        {
            if (typeof(T) == typeof(DataBasic))
            {
                return GetGroupId(
                    platformElement,
                    screen,
                    position,
                    SetupBasic.SingleUseLevers);
            }

            if (typeof(T) == typeof(DataCountdown))
            {
                return GetGroupId(
                    platformElement,
                    screen,
                    position,
                    SetupCountdown.SingleUseLevers);
            }

            throw new NotSupportedException($"Unsupported group data type: {typeof(T)}");
        }

        /// <summary>
        ///     Get the group id of the block that is at the position of the entity,
        ///     or specified link position.
        /// </summary>
        /// <param name="root">Root <see cref="XElement" /> specified link may be taken from.</param>
        /// <param name="screen">Screen this entity is to be created on.</param>
        /// <param name="position">Position this entity is to be created at.</param>
        /// <param name="blockGroups">Collection of <see cref="IBlockGroupId" />.</param>
        /// <returns>ID of the block at the position. 0 if no block exists at that the position.</returns>
        public static int GetGroupId(XElement root, int screen, Vector2 position,
            params Dictionary<int, IBlockGroupId>[] blockGroups)
        {
            var xel = root.Element("Link");
            int link;
            if (xel != null)
            {
                link = (int.TryParse(xel.Element("Screen")?.Value, out var screenResult) ? screenResult * 10000 : 0)
                       + (int.TryParse(xel.Element("X")?.Value, out var xResult) ? xResult * 100 : 0)
                       + (int.TryParse(xel.Element("Y")?.Value, out var yResult) ? yResult : 0);
            }
            else
            {
                link = ((screen + 1) * 10000) + ((int)(position.X / 8) * 100) + (int)(position.Y / 8);
            }

            foreach (var blockGroup in blockGroups)
            {
                if (blockGroup.TryGetValue(link, out var value))
                {
                    return value.GroupId;
                }
            }

            return 0;
        }

        /// <summary>
        ///     Helper to create a <see cref="Vector2" /> from an <see cref="XElement" />.
        ///     Indicates if creation failed since we do not continue creation if it did.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <param name="result">The parsed Vector2.</param>
        /// <returns><c>true</c> if the parse succeeded, <c>false</c> otherwise.</returns>
        public static bool TryParseVector2(XElement element, out Vector2 result)
        {
            result = default;

            var xElement = element.Element("X");
            var yElement = element.Element("Y");

            if (xElement == null || yElement == null)
            {
                return false;
            }

            if (!float.TryParse(xElement.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var x) ||
                !float.TryParse(yElement.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
            {
                return false;
            }

            result = new Vector2(x, y);
            return true;
        }
    }
}
