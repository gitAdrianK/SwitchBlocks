namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml.Linq;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities;
    using SwitchBlocks.Setups;
    using SwitchBlocks.Util;
    using SwitchBlocks.Util.Deserialization;
    using Curve = Util.Curve;

    public class FactoryDrawablesGroup
    {
        // There are no levers for both group types

        public enum BlockType
        {
            Group,
            Sequence,
        }

        /// <summary>
        /// Creates all drawbles for a given BlockType.
        /// Entities are added to the manager automatically, this will not return anything.
        /// </summary>
        /// <param name="blockType">What type of block should be created</param>
        public static void CreateDrawables<T>(BlockType blockType, EntityGroupLogic<T> entityGroupLogic) where T : IGroupDataProvider
        {
            var contentManager = Game1.instance.contentManager;

            var path = Path.Combine(
            contentManager.root,
                ModStrings.FOLDER,
                "platforms",
                blockType.ToString());
            if (!Directory.Exists(path))
            {
                return;
            }

            var files = Directory.GetFiles(path);
            if (!files.Any())
            {
                return;
            }

            var groups = GetGroups(blockType);
            GetPlatforms(path, files, blockType, groups, entityGroupLogic);
        }

        private static void GetPlatforms<T>(
            string path,
            string[] files,
            BlockType blockType,
            Dictionary<int, BlockGroup> groups,
            EntityGroupLogic<T> entityGroupLogic)
            where T : IGroupDataProvider
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
                                Curve = Enum.TryParse<Curve>(platformElement.Element("Animation")?.Element("Curve")?.Value, true, out var curve) ? curve : Curve.Linear,
                                Style = Enum.TryParse<Style>(platformElement.Element("Animation")?.Element("Style")?.Value, true, out var style) ? style : Style.Fade,
                            },
                            AnimationOut = new Animation
                            {
                                Curve = Enum.TryParse<Curve>(platformElement.Element("AnimationOut")?.Element("Curve")?.Value, true, out var curve2) ? curve2 : curve,
                                Style = Enum.TryParse<Style>(platformElement.Element("AnimationOut")?.Element("Style")?.Value, true, out var style2) ? style2 : style,
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
                                FPS = float.TryParse(xel.Element("FPS")?.Value, NumberStyles.Float, CultureInfo.InvariantCulture, out var parsedFloat) ? parsedFloat : 1.0f,
                                Frames = xel.Element("Frames")?.Elements("float").Select(f => float.Parse(f.Value, CultureInfo.InvariantCulture)).ToArray(),
                                RandomOffset = xel.Element("RandomOffset")?.Value == "true",
                                ResetWithLever = xel.Element("ResetWithLever")?.Value == "true",
                            };
                        };
                        // Group
                        int groupId;
                        switch (blockType)
                        {
                            case BlockType.Group:
                                groupId = GetGroupId(
                                    platformElement,
                                    screen,
                                    platform.Position,
                                    SetupGroup.BlocksGroupA,
                                    SetupGroup.BlocksGroupB,
                                    SetupGroup.BlocksGroupC,
                                    SetupGroup.BlocksGroupD);
                                break;
                            case BlockType.Sequence:
                                groupId = GetGroupId(
                                    platformElement,
                                    screen,
                                    platform.Position,
                                    SetupSequence.BlocksSequenceA,
                                    SetupSequence.BlocksSequenceB,
                                    SetupSequence.BlocksSequenceC,
                                    SetupSequence.BlocksSequenceD);
                                break;
                            default:
                                throw new NotImplementedException("Unknown Block Type, cannot get group ID!");
                        }
                        if (groupId == 0)
                        {
                            continue;
                        }
                        if (!groups.TryGetValue(groupId, out var group))
                        {
                            continue;
                        }
                        // Entity
                        if (platform.Sprites == null)
                        {
                            _ = new EntityDrawPlatform(platform, screen, group);
                        }
                        else
                        {
                            _ = new EntityDrawPlatformLoop(platform, screen, group);
                        }
                        entityGroupLogic.AddScreen(screen);
                    }
                }
            }
        }

        private static Dictionary<int, BlockGroup> GetGroups(BlockType blockType)
        {
            switch (blockType)
            {
                case BlockType.Group:
                    return DataGroup.Instance.Groups;
                case BlockType.Sequence:
                    return DataSequence.Instance.Groups;
                default:
                    throw new NotImplementedException("Unknown Block Type, cannot get groups!");
            }
        }

        private static int GetGroupId(XElement root, int screen, Vector2 position, params Dictionary<int, IBlockGroupId>[] blockGroups)
        {
            int link;
            var xel = root.Element("Link");
            if (xel != null)
            {
                link = (int.Parse(xel.Element("Screen").Value) * 10000)
                    + (int.Parse(xel.Element("X").Value) * 100)
                    + int.Parse(xel.Element("Y").Value);
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
