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
    using Util.Deserialization;

    public static class FactoryLevers
    {
        /// <summary>The regex for files.</summary>
        private static Regex Regex { get; } = new Regex(@"^levers(\d+).xml$");

        public static void CreateLevers(
            string xmlPath,
            string texturePath,
            IDataProvider data)
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
                if (root?.Name != "Levers")
                {
                    continue;
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
                        !float.TryParse(xElement.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var x) ||
                        !float.TryParse(yElement.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var y))
                    {
                        continue;
                    }

                    var lever = new Lever
                    {
                        Texture = Game1.instance.contentManager.Load<Texture2D>(textureFile),
                        Position = new Vector2(x, y),
                        IsForeground = leverElement.Element("IsForeground") != null,
                    };

                    _ = new EntityDrawLever(lever, screen, data);
                }
            }
        }

        /// <summary>
        ///     Creates <see cref="EntityDrawLever" />.
        /// </summary>
        /// <param name="path">Path to the files containing platform definitions.</param>
        /// <param name="files">Files inside the given path.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        public static void CreateLeversLegacy(
            string path,
            string[] files,
            IDataProvider data)
        {
            var regex = new Regex(@"^levers(\d+).xml$");

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
                    if (root?.Name != "Levers")
                    {
                        continue;
                    }

                    foreach (var leverElement in root.Elements("Lever"))
                    {
                        // Texture
                        var xel = leverElement.Element("Texture");
                        if (xel == null)
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
                        xel = leverElement.Element("Position");
                        if (xel == null)
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
                        // Lever
                        var lever = new Lever
                        {
                            Texture = texture,
                            Position = position,
                            IsForeground = leverElement.Element("IsForeground") != null,
                        };
                        _ = new EntityDrawLever(lever, screen, data);
                    }
                }
            }
        }
    }
}
