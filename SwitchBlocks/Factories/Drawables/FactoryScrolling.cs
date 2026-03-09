namespace SwitchBlocks.Factories.Drawables
{
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Data;
    using Entities;
    using JumpKing;
    using Microsoft.Xna.Framework.Graphics;
    using Util;
    using Util.Deserialization;

    public static class FactoryScrolling
    {
        /// <summary>The regex for sand files.</summary>
        private static Regex RegexSands { get; } = new Regex(@"^sands(\d+).xml$");

        /// <summary>The regex for conveyor files.</summary>
        private static Regex RegexConveyors { get; } = new Regex(@"^conveyors(\d+).xml$");

        /// <summary>The regex for platform files.</summary>
        private static Regex RegexPlatforms { get; } = new Regex(@"^platforms(\d+).xml$");

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatformSand" /> and <see cref="EntityDrawPlatformConveyor" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="data">Data for the entity.</param>
        /// <param name="entityLogic"><see cref="EntityLogic{T}" />.</param>
        /// <param name="isVertical">Scrolling is vertical or horizontal</param>
        /// <param name="isLegacy">If the names are still the legacy variant.</param>
        /// <typeparam name="T">A class implementing <see cref="IDataProvider" />.</typeparam>
        public static void CreatePlatformsScrolling<T>(
            string xmlPath,
            string texturePath,
            T data,
            EntityLogic<T> entityLogic,
            bool isVertical,
            bool isLegacy = false)
            where T : class, IDataProvider
        {
            if (!Directory.Exists(xmlPath) || !Directory.Exists(texturePath))
            {
                return;
            }

            foreach (var file in Directory.EnumerateFiles(xmlPath))
            {
                Match match;
                if (isLegacy)
                {
                    match = RegexPlatforms.Match(Path.GetFileName(file));
                }
                else
                {
                    match = isVertical
                        ? RegexSands.Match(Path.GetFileName(file))
                        : RegexConveyors.Match(Path.GetFileName(file));
                }

                if (!match.Success || !int.TryParse(match.Groups[1].Value, out var screenIndex))
                {
                    continue;
                }

                var screen = screenIndex - 1;
                if (screen < 0)
                {
                    continue;
                }

                // The only scrolling textures are sand and conveyors,
                // but legacy sand still uses the element names Platforms/Platform.
                string rootName;
                string elementsName;
                if (isLegacy)
                {
                    rootName = "Platforms";
                    elementsName = "Platform";
                }
                else
                {
                    rootName = isVertical ? "Sands" : "Conveyors";
                    elementsName = isVertical ? "Sand" : "Conveyor";
                }

                using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    var doc = XDocument.Load(fs);
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
                            IsForeground = XmlHelper.ParseElementBool(platformElement, "IsForeground"),
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
    }
}
