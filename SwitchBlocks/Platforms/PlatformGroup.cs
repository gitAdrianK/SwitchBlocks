namespace SwitchBlocks.Platforms
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Xml;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Util;

    /// <summary>
    /// Represents a subclass of platform, groupplatform with an additional grou idand optional link position. <br />
    /// </summary>
    public class PlatformGroup : Platform
    {
        // As long as the methods are static I can't really make use of the inheritance for those methods

        public int GroupId { get; private set; }

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of platforms as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>A dictionary containing lists of platforms with the screennumber they appear on as key</returns>
        public static Dictionary<int, List<PlatformGroup>> GetPlatformsDictonary(string subfolder,
            params Dictionary<int, IBlockGroupId>[] blocksGroups)
        {
            var contentManager = Game1.instance.contentManager;
            var sep = Path.DirectorySeparatorChar;
            var path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}{ModStrings.PLATFORMS}{sep}{subfolder}{sep}";

            if (!Directory.Exists(path))
            {
                return null;
            }
            var files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return null;
            }

            var dictionary = new Dictionary<int, List<PlatformGroup>>();
            var regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
            foreach (var xmlFilePath in files)
            {
                var xmlFile = xmlFilePath.Split(sep).Last();

                if (!regex.IsMatch(xmlFile))
                {
                    continue;
                }

                var document = new XmlDocument();
                document.Load(xmlFilePath);
                var xmlPlatforms = document.LastChild;

                if (xmlPlatforms.Name != ModStrings.XML_PLATFORMS)
                {
                    continue;
                }

                var screenNr = int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1;
                var platforms = GetPlatformList(xmlPlatforms,
                    path,
                    sep,
                    screenNr,
                    blocksGroups);
                if (platforms.Count != 0)
                {
                    dictionary.Add(screenNr, platforms);
                }
            }
            return dictionary.Count == 0 ? null : dictionary;
        }

        // I really should clean this up, flippin' duplicated mess

        /// <summary>
        /// Gets a list containing platforms, this list may be empty.
        /// </summary>
        /// <param name="xmlPlatforms">The platforms xml node</param>
        /// <param name="path">The path to the file</param>
        /// <param name="sep">Path separator</param>
        /// <returns>A list containing all successfully created platforms</returns>
        protected static List<PlatformGroup> GetPlatformList(XmlNode xmlPlatforms,
            string path,
            char sep,
            int screenNr,
            Dictionary<int, IBlockGroupId>[] blocksGroups)
        {
            var list = new List<PlatformGroup>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                var xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary;
                dictionary = Xml.MapNamesRequired(xmlPlatform,
                    ModStrings.TEXTURE,
                    ModStrings.POSITION);

                if (dictionary == null)
                {
                    continue;
                }

                var platform = new PlatformGroup();
                // Texture
                var filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.TEXTURE]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                platform.Texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                platform.Width = platform.Texture.Width;
                platform.Height = platform.Texture.Height;

                // Position
                var position = Xml.GetVector2(xmlPlatform[dictionary[ModStrings.POSITION]]);
                if (position == null)
                {
                    continue;
                }
                platform.Position = (Vector2)position;

                // Link
                var link = (screenNr * 10000) + ((int)(platform.Position.X / 8) * 100) + (int)(platform.Position.Y / 8);
                if (dictionary.ContainsKey(ModStrings.LINK_POSITION))
                {
                    var optionalLink = Xml.GetLink(xmlPlatform[dictionary[ModStrings.LINK_POSITION]]);
                    if (optionalLink == null)
                    {
                        continue;
                    }
                    link = (int)optionalLink;
                }


                foreach (var blockGroup in blocksGroups)
                {
                    if (blockGroup.ContainsKey(link))
                    {
                        platform.GroupId = blockGroup[link].GroupId;
                        goto Found;
                    }
                }
                continue;
                Found:

                // Start state
                platform.StartState = false;

                // Animation
                platform.Animation = new Animation
                {
                    AnimStyle = Animation.Style.Fade,
                    AnimCurve = Animation.Curve.Linear
                };
                if (dictionary.ContainsKey(ModStrings.ANIMATION))
                {
                    var animationNode = xmlPlatform[dictionary[ModStrings.ANIMATION]];
                    platform.Animation = Xml.GetAnimation(animationNode);
                }

                platform.AnimationOut = platform.Animation;
                if (dictionary.ContainsKey(ModStrings.ANIMATION_OUT))
                {
                    var animationNode = xmlPlatform[dictionary[ModStrings.ANIMATION_OUT]];
                    platform.AnimationOut = Xml.GetAnimation(animationNode);
                }

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}
