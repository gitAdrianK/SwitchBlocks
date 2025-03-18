namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Util;
    using SwitchBlocks.Util.Deserialization;
    using static SwitchBlocks.Util.Animation;
    using Curve = Util.Animation.Curve;

    public class FactoryDrawables
    {
        // Apparently XDocument is faster, but you'll have to do the elements yourself
        // And testing it, it does seem faster, or at least not slower, so we change to XDcoument
        // (here and all other instances, like Data/Cache/Resets)
        // in hopes of cleaning up the entity creation mess that was previously.
        // Lets hope there no framedrops this time around and we can keep this.

        public enum DrawType
        {
            Platforms,
            Levers,
        }

        public enum BlockType
        {
            Auto,
            Basic,
            Countdown,
            Jump,
            Sand,
        }

        /// <summary>
        /// Creates all drawbles for a given DrawType and BlockType.
        /// Entities are added to the manager automatically, this will not return anything.
        /// </summary>
        /// <param name="drawType">If the drawable is a lever or a platform</param>
        /// <param name="blockType">What type of block should be created</param>
        public static void CreateDrawables<T>(DrawType drawType, BlockType blockType, EntityLogic<T> entityLogic) where T : IDataProvider
        {
            var contentManager = Game1.instance.contentManager;

            var path = Path.Combine(
                contentManager.root,
                ModStrings.FOLDER,
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
                            GetPlatforms(path, files, data, entityLogic);
                            break;
                        case BlockType.Sand:
                            GetPlatformsSand(path, files, data, entityLogic);
                            break;
                        default:
                            throw new NotImplementedException("Unknown Block Type, cannot create entities!");
                    }
                    break;
                case DrawType.Levers:
                    GetLevers(path, files, data);
                    break;
                default:
                    throw new NotImplementedException("Unknown Draw Type, cannot create entities!");
            }
        }

        private static void GetPlatforms<T>(
            string path,
            string[] files,
            IDataProvider data,
            EntityLogic<T> entityLogic)
            where T : IDataProvider
        {
            var regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (!regex.IsMatch(fileName))
                {
                    continue;
                }

                var screen = int.Parse(Regex.Replace(fileName, @"[^\d]", "")) - 1;

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
                        var texturePath = Path.Combine(path, ModStrings.TEXTURES, xel.Value);
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
                            StartState = platformElement.Element("StartState")?.Value == "on",
                            Animation = new Animation
                            {
                                AnimCurve = Enum.TryParse<Curve>(platformElement.Element("Animation")?.Element("Curve")?.Value, true, out var curve) ? curve : Curve.Linear,
                                AnimStyle = Enum.TryParse<Style>(platformElement.Element("Animation")?.Element("Style")?.Value, true, out var style) ? style : Style.Fade,
                            },
                            AnimationOut = new Animation
                            {
                                AnimCurve = Enum.TryParse<Curve>(platformElement.Element("AnimationOut")?.Element("Curve")?.Value, true, out var curve2) ? curve2 : curve,
                                AnimStyle = Enum.TryParse<Style>(platformElement.Element("AnimationOut")?.Element("Style")?.Value, true, out var style2) ? style2 : style,
                            },
                            Sprites = null,
                        };
                        // Sprites
                        if ((xel = platformElement.Element("Sprites")) != null)
                        {
                            platform.Sprites = new Sprites
                            {
                                Cells = new Point
                                {
                                    X = int.TryParse(xel.Element("Cells")?.Element("X")?.Value, out var parsedInt) ? parsedInt : 1,
                                    Y = int.TryParse(xel.Element("Cells")?.Element("Y")?.Value, out parsedInt) ? parsedInt : 1,
                                },
                                FPS = int.TryParse(xel.Element("FPS")?.Value, out parsedInt) ? parsedInt : 1,
                                Frames = null,
                                RandomOffset = bool.TryParse(xel.Element("RandomOffset")?.Value, out var parsedBool) && parsedBool,
                                ResetWithLever = bool.TryParse(xel.Element("ResetWithLever")?.Value, out parsedBool) && parsedBool,
                            };
                            if ((xel = xel.Element("Frames")) != null)
                            {
                                var frames = new List<float>();
                                foreach (var framesElement in xel.Elements("float"))
                                {
                                    var frame = float.Parse(framesElement.Value, CultureInfo.InvariantCulture);
                                    frames.Add(frame);
                                }
                                platform.Sprites.Frames = frames;
                            }
                            // Entity
                            if (platform.Sprites.ResetWithLever)
                            {
                                // Specially hardcoded to work with countdown blocks only.
                                _ = new EntityDrawPlatformReset(platform, screen);
                            }
                            else
                            {
                                _ = new EntityDrawPlatformLoop(platform, screen, data);
                            }
                        }
                        else
                        {
                            _ = new EntityDrawPlatform(platform, screen, data);
                        }
                        entityLogic.AddScreen(screen);
                    };
                }
            }
        }

        private static void GetPlatformsSand<T>(
            string path,
            string[] files,
            IDataProvider data,
            EntityLogic<T> entityLogic)
            where T : IDataProvider
        {
            var regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (!regex.IsMatch(fileName))
                {
                    continue;
                }

                var screen = int.Parse(Regex.Replace(fileName, @"[^\d]", "")) - 1;

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
                        var textureFolder = Path.Combine(path, ModStrings.TEXTURES);
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
                            if (!File.Exists(texturePath + ".xnb"))
                            {
                                scrolling = Game1.instance.contentManager.Load<Texture2D>(texturePath);
                            }
                        }
                        // Foregorund
                        if ((xel = platformElement.Element("Foreground")) != null)
                        {
                            texturePath = Path.Combine(path, ModStrings.TEXTURES, xel.Value);
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
                        var platform = new PlatformSand
                        {
                            Background = background,
                            Scrolling = scrolling,
                            Foreground = foreground,
                            Position = position,
                            StartState = platformElement.Element("StartState")?.Value == "on",
                        };
                        _ = new EntityDrawPlatformSand(platform, screen, data);
                        entityLogic.AddScreen(screen);
                    };
                }
            }
        }


        private static void GetLevers(
            string path,
            string[] files,
            IDataProvider data)
        {
            var regex = new Regex(@"^levers(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");

            foreach (var file in files)
            {
                var fileName = Path.GetFileName(file);
                if (!regex.IsMatch(fileName))
                {
                    continue;
                }

                var screen = int.Parse(Regex.Replace(fileName, @"[^\d]", "")) - 1;

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
                        var texturePath = Path.Combine(path, ModStrings.TEXTURES, xel.Value);
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
                        };
                        _ = new EntityDrawLever(lever, screen, data);
                    };
                }
            }
        }

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
