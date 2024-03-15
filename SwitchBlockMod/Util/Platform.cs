using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace SwitchBlocksMod.Entities
{
    /// <summary>
    /// Represent a platform with a texture, position, and start state.
    /// </summary>
    public struct Platform
    {
        public Texture2D texture;
        public Vector2 position;
        public bool startState;

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of platforms as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>Dictionary&lt;int,List&lt;Platform&gt;&gt;</returns>
        public static Dictionary<int, List<Platform>> GetPlatformsDictonary(string subfolder)
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}switchBlocksMod{sep}platforms{sep}{subfolder}{sep}";
            Dictionary<int, List<Platform>> dictionary = new Dictionary<int, List<Platform>>();

            if (!Directory.Exists(path))
            {
                return dictionary;
            }

            Regex regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");

            foreach (string xmlFilePath in Directory.GetFiles(path))
            {
                string xmlFile = xmlFilePath.Split(sep).Last();

                if (!regex.IsMatch(xmlFile))
                {
                    continue;
                }

                XmlDocument document = new XmlDocument();
                document.Load(xmlFilePath);
                XmlNode xmlPlatforms = document.LastChild;

                if (xmlPlatforms.Name != "Platforms")
                {
                    return dictionary;
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
        /// <returns>List&lt;Platform&gt;</returns>
        private static List<Platform> GetPlatformList(XmlNode xmlPlatforms, string path, char sep)
        {
            List<Platform> list = new List<Platform>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                XmlNodeList xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary = Xml.MapNamesExact(xmlPlatform, "Texture", "Position", "StartState");
                if (dictionary == null)
                {
                    continue;
                }

                Platform platform;
                // CONSIDER: Using a stiched texture, and just saving a Rectangle w/ start xy, width, height, would probably be more performant.
                // Implement if needed.
                // Texture
                string filePath = $"{path}textures{sep}{xmlPlatform[dictionary["Texture"]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                platform.texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");

                // Position
                Vector2? position = Xml.GetVector2(xmlPlatform[dictionary["Position"]]);
                if (position == null)
                {
                    continue;
                }
                platform.position = (Vector2)position;

                // Start state
                string stateInnerText = xmlPlatform[dictionary["StartState"]].InnerText;
                if (stateInnerText == "on")
                {
                    platform.startState = true;
                }
                else if (stateInnerText == "off")
                {
                    platform.startState = false;
                }
                else
                {
                    // Yeah I am limiting it to on/off, what are you gonna do about it?
                    continue;
                }

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}