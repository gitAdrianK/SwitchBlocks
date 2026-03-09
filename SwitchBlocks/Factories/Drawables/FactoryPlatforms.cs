namespace SwitchBlocks.Factories.Drawables
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
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
    using Curve = Util.Curve;

    public static class FactoryPlatforms
    {
        /// <summary>The regex for files.</summary>
        private static Regex Regex { get; } = new Regex(@"^platforms(\d+).xml$");

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatform" />, <see cref="EntityDrawPlatformLoop" /> and
        ///     <see cref="EntityDrawPlatformReset" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="data">Data for the entity.</param>
        /// <param name="entityLogic"><see cref="EntityLogic{T}" />.</param>
        /// <typeparam name="T">>A class implementing <see cref="IDataProvider" />.</typeparam>
        public static void CreatePlatforms<T>(string xmlPath, string texturePath, T data, EntityLogic<T> entityLogic)
            where T : class, IDataProvider
        {
            if (!Directory.Exists(xmlPath) || !Directory.Exists(texturePath))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(xmlPath))
            {
                var match = Regex.Match(Path.GetFileName(file));
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
                    if (root == null || root.Name != "Platforms")
                    {
                        return;
                    }

                    foreach (var platformElement in root.Elements("Platform"))
                    {
                        var platform = TryParsePlatformElement(platformElement, texturePath);
                        if (platform == null)
                        {
                            continue;
                        }

                        var spritesElement = platformElement.Element("Sprites");
                        if (spritesElement != null && platform.Sprites.Cells != new Point(1, 1))
                        {
                            _ = platform.Sprites.ResetWithLever
                                ? new EntityDrawPlatformReset(platform, screen, data)
                                : new EntityDrawPlatformLoop(platform, screen, data);
                        }
                        else
                        {
                            _ = new EntityDrawPlatform(platform, screen, data);
                        }

                        entityLogic.AddScreen(screen);
                    }
                }
            }
        }

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatform" />, <see cref="EntityDrawPlatformLoop" /> and
        ///     <see cref="EntityDrawPlatformReset" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="groups">Collection of BlockGroups.</param>
        /// <param name="entityGroupLogic"><see cref="EntityGroupLogic{T}" />.</param>
        /// <typeparam name="T">>A class implementing <see cref="IGroupDataProvider" />.</typeparam>
        public static void CreateGroupPlatforms<T>(string xmlPath, string texturePath,
            Dictionary<int, BlockGroup> groups,
            EntityGroupLogic<T> entityGroupLogic)
            where T : IGroupDataProvider
        {
            if (!Directory.Exists(xmlPath) || !Directory.Exists(texturePath))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(xmlPath))
            {
                var match = Regex.Match(Path.GetFileName(file));
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
                    if (root == null || root.Name != "Platforms")
                    {
                        continue;
                    }

                    foreach (var platformElement in root.Elements("Platform"))
                    {
                        var platform = TryParsePlatformElement(platformElement, texturePath);
                        if (platform == null)
                        {
                            continue;
                        }

                        var groupId = FindGroupId<T>(platformElement, screen, platform.Position);
                        if (groupId == 0 || !groups.TryGetValue(groupId, out var group))
                        {
                            continue;
                        }

                        if (platform.Sprites != null)
                        {
                            _ = platform.Sprites.ResetWithLever
                                ? new EntityDrawPlatformReset(platform, screen, group)
                                : new EntityDrawPlatformLoop(platform, screen, group);
                        }
                        else
                        {
                            _ = new EntityDrawPlatform(platform, screen, group);
                        }

                        entityGroupLogic.AddScreen(screen);
                    }
                }
            }
        }

        /// <summary>
        ///     Helper to try to parse an <see cref="XElement" /> to a platform.
        /// </summary>
        /// <param name="platformElement">The element to create a platform from.</param>
        /// <param name="texturePath">Path to the texture.</param>
        /// <returns>A platform if creation was successful, <c>null</c> otherwise.</returns>
        private static Platform TryParsePlatformElement(XElement platformElement, string texturePath)
        {
            if (platformElement == null)
            {
                return null;
            }

            var textureElement = platformElement.Element("Texture");
            var positionElement = platformElement.Element("Position");

            if (textureElement == null || positionElement == null)
            {
                return null;
            }

            var textureFile = Path.Combine(texturePath, textureElement.Value);
            if (!File.Exists(textureFile + ".xnb"))
            {
                return null;
            }

            if (!TryParseVector2(positionElement, out var position))
            {
                return null;
            }

            var animation = ParseAnimation(platformElement.Element("Animation"), Curve.Linear, Style.Fade);
            var animationOut =
                ParseAnimation(platformElement.Element("AnimationOut"), animation.Curve, animation.Style);

            var platform = new Platform
            {
                Texture = Game1.instance.contentManager.Load<Texture2D>(textureFile),
                Position = position,
                StartState =
                    ParseEnum(
                        platformElement.Element("StartState") != null
                            ? platformElement.Element("StartState").Value
                            : null, StartState.Off),
                Animation = animation,
                AnimationOut = animationOut,
                IsForeground = XmlHelper.ParseElementBool(platformElement, "IsForeground"),
            };

            var spritesElement = platformElement.Element("Sprites");
            if (spritesElement == null)
            {
                return platform;
            }

            var sprites = ParseSprites(spritesElement);
            if (sprites.Cells != new Point(1, 1))
            {
                platform.Sprites = sprites;
            }

            return platform;
        }

        /// <summary>
        ///     Helper to find, for a block type, the group id.
        /// </summary>
        /// <param name="platformElement">The platform XElement.</param>
        /// <param name="screen">The screen.</param>
        /// <param name="position">The position.</param>
        /// <typeparam name="T">The <see cref="IGroupDataProvider" />.</typeparam>
        /// <returns>The found group id or 0 otherwise.</returns>
        /// <exception cref="NotSupportedException">Should the given type T not be supported.</exception>
        private static int FindGroupId<T>(
            XElement platformElement,
            int screen,
            Vector2 position)
            where T : IGroupDataProvider
        {
            if (typeof(T) == typeof(DataGroup))
            {
                return GetGroupId(
                    platformElement,
                    screen,
                    position,
                    SetupGroup.BlocksGroupA,
                    SetupGroup.BlocksGroupB,
                    SetupGroup.BlocksGroupC,
                    SetupGroup.BlocksGroupD);
            }

            if (typeof(T) == typeof(DataSequence))
            {
                return GetGroupId(
                    platformElement,
                    screen,
                    position,
                    SetupSequence.BlocksSequenceA,
                    SetupSequence.BlocksSequenceB,
                    SetupSequence.BlocksSequenceC,
                    SetupSequence.BlocksSequenceD);
            }

            throw new NotSupportedException($"Unsupported group data type: {typeof(T)}");
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

        /// <summary>
        ///     Helper to create an enum from a string.
        /// </summary>
        /// <param name="value">The string to parse.</param>
        /// <param name="fallback">Fallback should the parse fail.</param>
        /// <returns>Parsed enum.</returns>
        public static T ParseEnum<T>(string value, T fallback)
            where T : struct =>
            Enum.TryParse(value, true, out T parsed)
                ? parsed
                : fallback;

        /// <summary>
        ///     Helper to create an <see cref="Animation" /> from an <see cref="XElement" />.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <param name="defaultCurve">The default curve should none be specified.</param>
        /// <param name="defaultStyle">The default style should none be specified.</param>
        /// <returns>Animation.</returns>
        private static Animation ParseAnimation(
            XElement element,
            Curve defaultCurve,
            Style defaultStyle)
        {
            if (element == null)
            {
                return new Animation { Curve = defaultCurve, Style = defaultStyle };
            }

            return new Animation
            {
                Curve = ParseEnum(element.Element("Curve")?.Value, defaultCurve),
                Style = ParseEnum(element.Element("Style")?.Value, defaultStyle),
            };
        }

        /// <summary>
        ///     Helper to create a <see cref="Sprites" /> object from an <see cref="XElement" />.
        /// </summary>
        /// <param name="element">The XElement.</param>
        /// <returns>Sprites.</returns>
        private static Sprites ParseSprites(XElement element)
        {
            var cellsElement = element.Element("Cells");

            return new Sprites
            {
                Cells = new Point(
                    TryParseInt(cellsElement?.Element("X"), 2),
                    TryParseInt(cellsElement?.Element("Y"), 2)
                ),
                Fps = TryParseFloat(element.Element("FPS"), 1f),
                Frames = element.Element("Frames")?
                    .Elements("float")
                    .Select(f => float.Parse(f.Value, CultureInfo.InvariantCulture))
                    .ToArray(),
                RandomOffset = XmlHelper.ParseElementBool(element, "RandomOffset"),
                ResetWithLever = XmlHelper.ParseElementBool(element, "ResetWithLever"),
                IgnoreState = XmlHelper.ParseElementBool(element, "IgnoreState"),
            };

            int TryParseInt(XElement el, int fallback) =>
                int.TryParse(el?.Value, out var val) ? val : fallback;

            float TryParseFloat(XElement el, float fallback) =>
                float.TryParse(el?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var val)
                    ? val
                    : fallback;
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
    }
}
