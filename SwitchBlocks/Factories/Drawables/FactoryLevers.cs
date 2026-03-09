namespace SwitchBlocks.Factories.Drawables
{
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

    public static class FactoryLevers
    {
        /// <summary>The regex for files.</summary>
        private static Regex Regex { get; } = new Regex(@"^levers(\d+).xml$");

        /// <summary>
        ///     Creates <see cref="EntityDrawLever" />.
        /// </summary>
        /// <param name="xmlPath">Path to XML files.</param>
        /// <param name="texturePath">Path to textures.</param>
        /// <param name="data">Data for the entity.</param>
        public static void CreateLevers(
            string xmlPath,
            string texturePath,
            IDataProvider data)
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
                    if (root?.Name != "Levers")
                    {
                        return;
                    }

                    foreach (var leverElement in root.Elements("Lever"))
                    {
                        var textureElement = leverElement.Element("Texture");
                        var positionElement = leverElement.Element("Position");

                        if (textureElement == null || positionElement == null)
                        {
                            continue;
                        }

                        var textureFile = Path.Combine(texturePath, textureElement.Value);
                        if (!File.Exists(textureFile + ".xnb"))
                        {
                            continue;
                        }

                        var xElement = positionElement.Element("X");
                        var yElement = positionElement.Element("Y");

                        if (xElement == null || yElement == null ||
                            !float.TryParse(xElement.Value, NumberStyles.Float, CultureInfo.InvariantCulture,
                                out var x) ||
                            !float.TryParse(yElement.Value, NumberStyles.Float, CultureInfo.InvariantCulture,
                                out var y))
                        {
                            continue;
                        }

                        var lever = new Lever
                        {
                            Texture = Game1.instance.contentManager.Load<Texture2D>(textureFile),
                            Position = new Vector2(x, y),
                            IsForeground = XmlHelper.ParseElementBool(leverElement, "IsForeground"),
                        };

                        _ = new EntityDrawLever(lever, screen, data);
                    }
                }
            }
        }
    }
}
