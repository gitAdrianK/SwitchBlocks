namespace SwitchBlocks.Factories
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using Data;
    using Entities;
    using EntityComponent;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util;
    using Util.Deserialization;
    using Curve = Util.Curve;

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
        public static void CreateDrawables<T>(DrawType drawType, BlockType blockType, EntityLogic<T> entityLogic)
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
                            CreatePlatforms(path, files, data, entityLogic);
                            break;
                        case BlockType.Sand:
                            CreatePlatformsSand(path, files, data, entityLogic);
                            break;
                        default:
                            throw new NotImplementedException("Unknown Block Type, cannot create entities!");
                    }

                    break;
                case DrawType.Levers:
                    CreateLevers(path, files, data);
                    break;
                default:
                    throw new NotImplementedException("Unknown Draw Type, cannot create entities!");
            }
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
        private static void CreatePlatforms<T>(
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
                        if ((xel = platformElement.Element("Texture")) is null)
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
                        if ((xel = platformElement.Element("Position")) is null)
                        {
                            continue;
                        }

                        var x = xel.Element("X");
                        var y = xel.Element("Y");
                        if (x is null || y is null)
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
                            IsForeground = !(platformElement.Element("IsForeground") is null),
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

        /// <summary>
        ///     Creates <see cref="EntityDrawPlatformSand" />.
        /// </summary>
        /// <typeparam name="T">A class implementing <see cref="IDataProvider" />.</typeparam>
        /// <param name="path">Path to the files containing platform definitions.</param>
        /// <param name="files">Files inside the given path.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        /// <param name="entityLogic"><see cref="EntityLogic{T}" />.</param>
        private static void CreatePlatformsSand<T>(
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
                        if (background is null && foreground is null)
                        {
                            continue;
                        }

                        // Position
                        if ((xel = platformElement.Element("Position")) is null)
                        {
                            continue;
                        }

                        var x = xel.Element("X");
                        var y = xel.Element("Y");
                        if (x is null || y is null)
                        {
                            continue;
                        }

                        var position = new Vector2
                        {
                            X = float.Parse(x.Value, CultureInfo.InvariantCulture),
                            Y = float.Parse(y.Value, CultureInfo.InvariantCulture),
                        };
                        // Platform
                        var platform = new PlatformSand
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
                            IsForeground = !(platformElement.Element("IsForeground") is null),
                        };
                        _ = new EntityDrawPlatformSand(platform, screen, data);
                        entityLogic.AddScreen(screen);
                    }
                }
            }
        }

        /// <summary>
        ///     Creates <see cref="EntityDrawLever" />.
        /// </summary>
        /// <param name="path">Path to the files containing platform definitions.</param>
        /// <param name="files">Files inside the given path.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        private static void CreateLevers(
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
                        if (xel is null)
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
                        if (xel is null)
                        {
                            continue;
                        }

                        var x = xel.Element("X");
                        var y = xel.Element("Y");
                        if (x is null || y is null)
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
                            IsForeground = !(leverElement.Element("IsForeground") is null),
                        };
                        _ = new EntityDrawLever(lever, screen, data);
                    }
                }
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
