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

        public static void CreatePlatforms<T>(
            string xmlPath,
            string texturePath,
            T data,
            EntityLogic<T> entityLogic)
            where T : class, IDataProvider
        {
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

                var doc = XDocument.Load(file);
                var root = doc.Root;
                if (root?.Name != "Platforms")
                {
                    continue;
                }

                foreach (var platformElement in root.Elements("Platform"))
                {
                    var textureElement = platformElement.Element("Texture");
                    var positionElement = platformElement.Element("Position");

                    if (textureElement == null || positionElement == null)
                    {
                        continue;
                    }

                    var textureFile = Path.Combine(texturePath, textureElement.Value);
                    if (!File.Exists(textureFile + ".xnb"))
                    {
                        continue;
                    }

                    if (!TryParseVector2(positionElement, out var position))
                    {
                        continue;
                    }

                    var animationElement = platformElement.Element("Animation");
                    var animationOutElement = platformElement.Element("AnimationOut");

                    var animation = ParseAnimation(animationElement, Curve.Linear, Style.Fade);
                    var animationOut = ParseAnimation(
                        animationOutElement,
                        animation.Curve,
                        animation.Style);

                    var platform = new Platform
                    {
                        Texture = Game1.instance.contentManager.Load<Texture2D>(textureFile),
                        Position = position,
                        StartState = ParseEnum(platformElement.Element("StartState")?.Value, StartState.Off),
                        Animation = animation,
                        AnimationOut = animationOut,
                        IsForeground = platformElement.Element("IsForeground") != null,
                    };

                    var spritesElement = platformElement.Element("Sprites");
                    if (spritesElement != null)
                    {
                        platform.Sprites = ParseSprites(spritesElement);

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

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatform" />, <see cref="EntityDrawPlatformLoop" /> and
        ///     <see cref="EntityDrawPlatformReset" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="groups">Collection of BlockGroups.</param>
        /// <param name="entityGroupLogic"><see cref="EntityGroupLogic{T}" />.</param>
        /// <typeparam name="T">>A class implementing <see cref="IGroupDataProvider" />.</typeparam>
        public static void CreatePlatforms<T>(
            string xmlPath,
            string texturePath,
            Dictionary<int, BlockGroup> groups,
            EntityGroupLogic<T> entityGroupLogic)
            where T : IGroupDataProvider
        {
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

                var doc = XDocument.Load(file);
                var root = doc.Root;
                if (root?.Name != "Platforms")
                {
                    continue;
                }

                foreach (var platformElement in root.Elements("Platform"))
                {
                    var textureElement = platformElement.Element("Texture");
                    if (textureElement == null)
                    {
                        continue;
                    }

                    var textureFile = Path.Combine(texturePath, textureElement.Value);
                    if (!File.Exists(textureFile + ".xnb"))
                    {
                        continue;
                    }

                    if (!TryParseVector2(platformElement.Element("Position"), out var position))
                    {
                        continue;
                    }

                    var animation = ParseAnimation(
                        platformElement.Element("Animation"),
                        Curve.Linear,
                        Style.Fade);

                    var animationOut = ParseAnimation(
                        platformElement.Element("AnimationOut"),
                        animation.Curve,
                        animation.Style);

                    var platform = new Platform
                    {
                        Texture = Game1.instance.contentManager.Load<Texture2D>(textureFile),
                        Position = position,
                        StartState = ParseEnum(
                            platformElement.Element("StartState")?.Value,
                            StartState.Off),
                        Animation = animation,
                        AnimationOut = animationOut,
                        IsForeground = platformElement.Element("IsForeground") != null,
                    };

                    var spritesElement = platformElement.Element("Sprites");
                    if (spritesElement != null)
                    {
                        platform.Sprites = ParseSprites(spritesElement);
                    }

                    var groupId = FindGroupId<T>(platformElement, screen, position);
                    if (groupId == 0)
                    {
                        continue;
                    }

                    if (!groups.TryGetValue(groupId, out var group))
                    {
                        continue;
                    }

                    _ = platform.Sprites == null
                        ? new EntityDrawPlatform(platform, screen, group)
                        : new EntityDrawPlatformLoop(platform, screen, group);

                    entityGroupLogic.AddScreen(screen);
                }
            }
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
                return FactoryDrawablesGroup.GetGroupId(
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
                return FactoryDrawablesGroup.GetGroupId(
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
                    TryParseInt(cellsElement?.Element("X"), 1),
                    TryParseInt(cellsElement?.Element("Y"), 1)
                ),
                Fps = TryParseFloat(element.Element("FPS"), 1f),
                Frames = element.Element("Frames")?
                    .Elements("float")
                    .Select(f => float.Parse(f.Value, CultureInfo.InvariantCulture))
                    .ToArray(),
                RandomOffset = TryParseBool(element.Element("RandomOffset")),
                ResetWithLever = TryParseBool(element.Element("ResetWithLever")),
                IgnoreState = TryParseBool(element.Element("IgnoreState")),
            };

            int TryParseInt(XElement el, int fallback) =>
                int.TryParse(el?.Value, out var val) ? val : fallback;

            bool TryParseBool(XElement el) =>
                bool.TryParse(el?.Value, out var val) && val;

            float TryParseFloat(XElement el, float fallback) =>
                float.TryParse(el?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var val)
                    ? val
                    : fallback;
        }

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatform" />, <see cref="EntityDrawPlatformLoop" /> and
        ///     <see cref="EntityDrawPlatformReset" />.
        /// </summary>
        /// <typeparam name="T">A class implementing <see cref="IDataProvider" />.</typeparam>
        /// <param name="path">Path to the files containing platform definitions.</param>
        /// <param name="files">Files inside the given path.</param>
        /// <param name="data">Data provider.</param>
        /// <param name="entityLogic">
        ///     <see cref="EntityLogic{T}" />.
        /// </param>
        public static void CreatePlatformsLegacy<T>(
            string path,
            string[] files,
            IDataProvider data,
            EntityLogic<T> entityLogic)
            where T : class, IDataProvider
        {
            var regex = new Regex(@"^platforms(\d+).xml$");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                var match = regex.Match(fileName);
                if (!match.Success)
                {
                    continue;
                }

                var screen = int.Parse(match.Groups[1].Value) - 1;
                if (screen < 0)
                {
                    continue;
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
                    var root = doc.Root;
                    if (root?.Name != "Platforms")
                    {
                        continue;
                    }

                    foreach (var platformElement in root.Elements("Platform"))
                    {
                        // Texture
                        XElement xel;
                        if ((xel = platformElement.Element("Texture")) == null)
                        {
                            continue;
                        }

                        var texturePath = Path.Combine(path, ModConstants.Textures, xel.Value);
                        if (!File.Exists(texturePath + ".xnb"))
                        {
                            continue;
                        }

                        var texture = Game1.instance.contentManager.Load<Texture2D>(texturePath);
                        // Position
                        if ((xel = platformElement.Element("Position")) == null)
                        {
                            continue;
                        }

                        var x = xel.Element("X");
                        var y = xel.Element("Y");
                        if (x == null || y == null)
                        {
                            continue;
                        }

                        var position = new Vector2
                        {
                            X = float.Parse(x.Value, CultureInfo.InvariantCulture),
                            Y = float.Parse(y.Value, CultureInfo.InvariantCulture),
                        };
                        // Platform
                        var platform = new Platform
                        {
                            Texture = texture,
                            Position = position,
                            StartState = Enum.TryParse<StartState>(
                                platformElement.Element("StartState")?.Value, true,
                                out var startState)
                                ? startState
                                : StartState.Off,
                            Animation = new Animation
                            {
                                Curve =
                                    Enum.TryParse<Curve>(
                                        platformElement.Element("Animation")?.Element("Curve")?.Value, true,
                                        out var curve)
                                        ? curve
                                        : Curve.Linear,
                                Style =
                                    Enum.TryParse<Style>(
                                        platformElement.Element("Animation")?.Element("Style")?.Value, true,
                                        out var style)
                                        ? style
                                        : Style.Fade,
                            },
                            AnimationOut = new Animation
                            {
                                Curve =
                                    Enum.TryParse<Curve>(
                                        platformElement.Element("AnimationOut")?.Element("Curve")?.Value, true,
                                        out var curve2)
                                        ? curve2
                                        : curve,
                                Style = Enum.TryParse<Style>(
                                    platformElement.Element("AnimationOut")?.Element("Style")?.Value, true,
                                    out var style2)
                                    ? style2
                                    : style,
                            },
                            IsForeground = platformElement.Element("IsForeground") != null,
                            Sprites = null,
                        };
                        // Sprites
                        if ((xel = platformElement.Element("Sprites")) != null)
                        {
                            platform.Sprites = new Sprites
                            {
                                Cells = new Point
                                {
                                    X = int.TryParse(xel.Element("Cells")?.Element("X")?.Value,
                                        out var parsedInt)
                                        ? parsedInt
                                        : 1,
                                    Y =
                                        int.TryParse(xel.Element("Cells")?.Element("Y")?.Value, out parsedInt)
                                            ? parsedInt
                                            : 1,
                                },
                                Fps =
                                    float.TryParse(xel.Element("FPS")?.Value, NumberStyles.Float,
                                        CultureInfo.InvariantCulture, out var fps)
                                        ? fps
                                        : 1.0f,
                                Frames =
                                    xel.Element("Frames")?.Elements("float").Select(f =>
                                        float.Parse(f.Value, CultureInfo.InvariantCulture)).ToArray(),
                                RandomOffset =
                                    bool.TryParse(xel.Element("RandomOffset")?.Value, out var parsedBool) &&
                                    parsedBool,
                                ResetWithLever =
                                    bool.TryParse(xel.Element("ResetWithLever")?.Value, out parsedBool) &&
                                    parsedBool,
                                IgnoreState =
                                    bool.TryParse(xel.Element("IgnoreState")?.Value, out parsedBool) &&
                                    parsedBool,
                            };
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
    }
}
