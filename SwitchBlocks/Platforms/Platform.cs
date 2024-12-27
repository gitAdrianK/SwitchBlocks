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
    /// Represents a platform with a texture, position, size, and start state.
    /// </summary>
    public class Platform
    {
        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get; protected set; }
        public int Height { get; protected set; }
        public int Width { get; protected set; }
        public bool StartState { get; protected set; }
        public Animation Animation { get; protected set; }
        public Animation AnimationOut { get; protected set; }

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of platforms as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>A dictionary containing lists of platforms with the screennumber they appear on as key</returns>
        public static Dictionary<int, List<Platform>> GetPlatformsDictonary(string subfolder)
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

            var dictionary = new Dictionary<int, List<Platform>>();
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

                var platforms = GetPlatformList(xmlPlatforms, path, sep);
                if (platforms.Count != 0)
                {
                    dictionary.Add(int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1, platforms);
                }

            }
            return dictionary.Count == 0 ? null : dictionary;
        }

        /// <summary>
        /// Gets a list containing platforms, this list may be empty.
        /// </summary>
        /// <param name="xmlPlatforms">The platforms xml node</param>
        /// <param name="path">The path to the file</param>
        /// <param name="sep">Path separator</param>
        /// <returns>A list containing all successfully created platforms</returns>
        protected static List<Platform> GetPlatformList(XmlNode xmlPlatforms, string path, char sep)
        {
            var list = new List<Platform>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                var xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary;
                dictionary = Xml.MapNamesRequired(xmlPlatform,
                    ModStrings.TEXTURE,
                    ModStrings.POSITION,
                    ModStrings.START_STATE);

                if (dictionary == null)
                {
                    continue;
                }

                var platform = new Platform();
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
                if (!position.HasValue)
                {
                    continue;
                }
                platform.Position = position.Value;

                // Start state
                var stateInnerText = xmlPlatform[dictionary[ModStrings.START_STATE]].InnerText.ToLower();
                if (stateInnerText == "on")
                {
                    platform.StartState = true;
                }
                else if (stateInnerText == "off")
                {
                    platform.StartState = false;
                }
                else
                {
                    // Yeah I am limiting it to on/off, what are you gonna do about it?
                    continue;
                }

                // Animation
                platform.Animation = new Animation
                {
                    AnimStyle = Animation.Style.Fade,
                    AnimCurve = Animation.Curve.Linear
                };
                if (dictionary.TryGetValue(ModStrings.ANIMATION, out var value))
                {
                    platform.Animation = Xml.GetAnimation(xmlPlatform[value]);
                }

                platform.AnimationOut = platform.Animation;
                if (dictionary.TryGetValue(ModStrings.ANIMATION_OUT, out value))
                {
                    platform.AnimationOut = Xml.GetAnimation(xmlPlatform[value]);
                }

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}
