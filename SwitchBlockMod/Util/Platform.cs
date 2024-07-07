using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace SwitchBlocksMod.Util
{
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
        private Animation animation;

        public Animation Animation => animation;

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of platforms as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>A dictionary containing lists of platforms with the screennumber they appear on as key</returns>
        public static Dictionary<int, List<Platform>> GetPlatformsDictonary(string subfolder)
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}{ModStrings.PLATFORMS}{sep}{subfolder}{sep}";

            if (!Directory.Exists(path))
            {
                return null;
            }

            string[] files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return null;
            }

            Regex regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
            Dictionary<int, List<Platform>> dictionary = new Dictionary<int, List<Platform>>();
            foreach (string xmlFilePath in files)
            {
                string xmlFile = xmlFilePath.Split(sep).Last();

                if (!regex.IsMatch(xmlFile))
                {
                    continue;
                }

                XmlDocument document = new XmlDocument();
                document.Load(xmlFilePath);
                XmlNode xmlPlatforms = document.LastChild;

                if (xmlPlatforms.Name != ModStrings.XML_PLATFORMS)
                {
                    continue;
                }

                List<Platform> platforms = GetPlatformList(xmlPlatforms, path, sep);
                if (platforms.Count != 0)
                {
                    dictionary.Add(int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1, platforms);
                }

            }
            return dictionary;
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
            List<Platform> list = new List<Platform>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                XmlNodeList xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary;
                dictionary = Xml.MapNamesRequired(xmlPlatform,
                    ModStrings.TEXTURE,
                    ModStrings.POSITION,
                    ModStrings.START_STATE);

                if (dictionary == null)
                {
                    continue;
                }

                Platform platform = new Platform();
                // Texture
                string filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.TEXTURE]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                platform.Texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                platform.Width = platform.Texture.Width;
                platform.Height = platform.Texture.Height;

                // Position
                Vector2? position = Xml.GetVector2(xmlPlatform[dictionary[ModStrings.POSITION]]);
                if (position == null)
                {
                    continue;
                }
                platform.Position = (Vector2)position;

                // Start state
                string stateInnerText = xmlPlatform[dictionary[ModStrings.START_STATE]].InnerText;
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
                platform.animation.style = Animation.Style.Fade;
                platform.animation.curve = Animation.Curve.Linear;
                if (dictionary.ContainsKey(ModStrings.ANIMATION))
                {
                    XmlNode animationNode = xmlPlatform[dictionary[ModStrings.ANIMATION]];
                    platform.animation = Xml.GetAnimation(animationNode);
                }

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}