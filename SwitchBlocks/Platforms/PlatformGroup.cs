using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace SwitchBlocks.Platforms
{
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
        public static new Dictionary<int, List<PlatformGroup>> GetPlatformsDictonary(string subfolder)
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

            Dictionary<int, List<PlatformGroup>> dictionary = new Dictionary<int, List<PlatformGroup>>();
            Regex regex = new Regex(@"^platforms(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
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

                int screenNr = int.Parse(Regex.Replace(xmlFile, @"[^\d]", ""));
                List<PlatformGroup> platforms = GetPlatformList(xmlPlatforms, path, sep, screenNr);
                if (platforms.Count != 0)
                {
                    dictionary.Add(screenNr - 1, platforms);
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
        protected static List<PlatformGroup> GetPlatformList(XmlNode xmlPlatforms, string path, char sep, int screenNr)
        {
            List<PlatformGroup> list = new List<PlatformGroup>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                XmlNodeList xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary;
                dictionary = Xml.MapNamesRequired(xmlPlatform,
                    ModStrings.TEXTURE,
                    ModStrings.POSITION);

                if (dictionary == null)
                {
                    continue;
                }

                string filePath;
                // Require at least one of the size giving textures to exist (Background or Foregroud)
                if (!dictionary.ContainsKey(ModStrings.BACKGROUND) && !dictionary.ContainsKey(ModStrings.FOREGROUND))
                {
                    continue;
                }

                PlatformGroup platform = new PlatformGroup();
                // Background
                if (dictionary.ContainsKey(ModStrings.BACKGROUND))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.BACKGROUND]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                    platform.Width = platform.Texture.Width;
                    platform.Height = platform.Texture.Height;
                }

                // Position
                Vector2? position = Xml.GetVector2(xmlPlatform[dictionary[ModStrings.POSITION]]);
                if (position == null)
                {
                    continue;
                }
                platform.Position = (Vector2)position;

                // TODO: Check if position relates to a group block and the id can be taken from there 

                // Start state
                platform.StartState = false;

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}
