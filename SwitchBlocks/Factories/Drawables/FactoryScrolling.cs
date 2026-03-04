namespace SwitchBlocks.Factories.Drawables
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Data;
    using Entities;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util;
    using Util.Deserialization;

    public static class FactoryScrolling
    {
        /// <summary>The regex for sand files.</summary>
        private static Regex RegexSands { get; } = new Regex(@"^sands(\d+).xml$");

        /// <summary>The regex for conveyor files.</summary>
        private static Regex RegexConveyors { get; } = new Regex(@"^conveyors(\d+).xml$");

        public static void CreatePlatformsScrolling<T>(
            string xmlPath,
            string texturePath,
            T data,
            EntityLogic<T> entityLogic,
            bool isVertical)
            where T : class, IDataProvider
        {
            foreach (var file in Directory.EnumerateFiles(xmlPath))
            {
                var match = isVertical
                    ? RegexSands.Match(Path.GetFileName(file))
                    : RegexConveyors.Match(Path.GetFileName(file));
                if (!match.Success || !int.TryParse(match.Groups[1].Value, out var screenIndex))
                {
                    continue;
                }

                var screen = screenIndex - 1;
                if (screen < 0)
                {
                    continue;
                }

                var rootName = isVertical ? "Sands" : "Conveyors";
                var elementsName = isVertical ? "Sand" : "Conveyor";
                var doc = XDocument.Load(file);
                var root = doc.Root;
                if (root?.Name != rootName)
                {
                    continue;
                }

                foreach (var platformElement in root.Elements(elementsName))
                {
                    var background = TryLoadTexture(platformElement, "Background", texturePath);
                    var scrolling = TryLoadTexture(platformElement, "Scrolling", texturePath);
                    var foreground = TryLoadTexture(platformElement, "Foreground", texturePath);

                    // At least one size giving texture required.
                    if (background == null && foreground == null)
                    {
                        continue;
                    }

                    if (!FactoryPlatforms.TryParseVector2(platformElement.Element("Position"), out var position))
                    {
                        continue;
                    }

                    var platform = new PlatformScrolling
                    {
                        Background = background,
                        Scrolling = scrolling,
                        Foreground = foreground,
                        Position = position,
                        StartState = FactoryPlatforms.ParseEnum(
                            platformElement.Element("StartState")?.Value,
                            StartState.On),
                        IsForeground = platformElement.Element("IsForeground") != null,
                        Multiplier = float.TryParse(
                            platformElement.Element("Multiplier")?.Value,
                            NumberStyles.Float,
                            CultureInfo.InvariantCulture,
                            out var parsed)
                            ? parsed
                            : 1.0f,
                    };

                    if (isVertical)
                    {
                        _ = new EntityDrawPlatformSand(platform, screen, data);
                    }
                    else
                    {
                        _ = new EntityDrawPlatformConveyor(platform, screen, data);
                    }

                    entityLogic.AddScreen(screen);
                }
            }
        }

        /// <summary>
        ///     Helper to load a texture.
        /// </summary>
        /// <param name="parent">The XElement that contains the potential texture field.</param>
        /// <param name="elementName">Name of the field that contains the texture name.</param>
        /// <param name="texturePath">Path to the folder containing the texture.</param>
        /// <returns>A <see cref="Texture2D" /> if successful, <c>null</c> otherwise.</returns>
        private static Texture2D TryLoadTexture(
            XElement parent,
            string elementName,
            string texturePath)
        {
            var element = parent.Element(elementName);
            if (element == null)
            {
                return null;
            }

            var textureFile = Path.Combine(texturePath, element.Value);
            return File.Exists(textureFile + ".xnb")
                ? Game1.instance.contentManager.Load<Texture2D>(textureFile)
                : null;
        }

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatformSand" />.
        /// </summary>
        /// <typeparam name="T">A class implementing <see cref="IDataProvider" />.</typeparam>
        /// <param name="path">Path to the files containing platform definitions.</param>
        /// <param name="files">Files inside the given path.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        /// <param name="entityLogic"><see cref="EntityLogic{T}" />.</param>
        /// <param name="isVertical">If the scroll is vertical or horizontal.</param>
        public static void CreatePlatformsScrollingLegacy<T>(
            string path,
            string[] files,
            IDataProvider data,
            EntityLogic<T> entityLogic,
            bool isVertical)
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
                        var textureFolder = Path.Combine(path, ModConstants.Textures);
                        string texturePath;
                        Texture2D background = null;
                        Texture2D scrolling = null;
                        Texture2D foreground = null;
                        // Background
                        XElement xel;
                        if ((xel = platformElement.Element("Background")) != null)
                        {
                            texturePath = Path.Combine(textureFolder, xel.Value);
                            if (File.Exists(texturePath + ".xnb"))
                            {
                                background = Game1.instance.contentManager.Load<Texture2D>(texturePath);
                            }
                        }

                        // Scrolling
                        if ((xel = platformElement.Element("Scrolling")) != null)
                        {
                            texturePath = Path.Combine(textureFolder, xel.Value);
                            if (File.Exists(texturePath + ".xnb"))
                            {
                                scrolling = Game1.instance.contentManager.Load<Texture2D>(texturePath);
                            }
                        }

                        // Foreground
                        if ((xel = platformElement.Element("Foreground")) != null)
                        {
                            texturePath = Path.Combine(path, ModConstants.Textures, xel.Value);
                            if (File.Exists(texturePath + ".xnb"))
                            {
                                foreground = Game1.instance.contentManager.Load<Texture2D>(texturePath);
                            }
                        }

                        // Min one size giving texture
                        if (background == null && foreground == null)
                        {
                            continue;
                        }

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
                        var platform = new PlatformScrolling
                        {
                            Background = background,
                            Scrolling = scrolling,
                            Foreground = foreground,
                            Position = position,
                            StartState = Enum.TryParse<StartState>(
                                platformElement.Element("StartState")?.Value, true,
                                out var startState)
                                ? startState
                                : StartState.On,
                            IsForeground = platformElement.Element("IsForeground") != null,
                            Multiplier = float.TryParse(platformElement.Element("Multiplier")?.Value,
                                NumberStyles.Float,
                                CultureInfo.InvariantCulture, out var multiplier)
                                ? multiplier
                                : 1.0f,
                        };

                        if (isVertical)
                        {
                            _ = new EntityDrawPlatformSand(platform, screen, data);
                        }
                        else
                        {
                            _ = new EntityDrawPlatformConveyor(platform, screen, data);
                        }

                        entityLogic.AddScreen(screen);
                    }
                }
            }
        }
    }
}
