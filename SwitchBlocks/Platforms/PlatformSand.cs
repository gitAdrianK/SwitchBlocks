using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace SwitchBlocks.Platforms
{
    /// <summary>
    /// Represents a subclass of platform, sandplatform with two more textures, scrolling and foreground. <br />
    /// Having at least a background OR foreground is required.
    /// </summary>
    public class PlatformSand : Platform
    {
        // As long as the methods are static I can't really make use of the inheritance for those methods

        public Texture2D Scrolling { get; private set; }
        public Texture2D Foreground { get; private set; }

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of platforms as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>A dictionary containing lists of platforms with the screennumber they appear on as key</returns>
        public static new Dictionary<int, List<PlatformSand>> GetPlatformsDictonary(string subfolder)
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

            Dictionary<int, List<PlatformSand>> dictionary = new Dictionary<int, List<PlatformSand>>();
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

                List<PlatformSand> platforms = GetPlatformList(xmlPlatforms, path, sep);
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
        protected static new List<PlatformSand> GetPlatformList(XmlNode xmlPlatforms, string path, char sep)
        {
            List<PlatformSand> list = new List<PlatformSand>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                XmlNodeList xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary;
                dictionary = Xml.MapNamesRequired(xmlPlatform,
                    ModStrings.POSITION,
                    ModStrings.START_STATE);

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

                PlatformSand platform = new PlatformSand();
                JKContentManager contentManager = Game1.instance.contentManager;
                // Background
                if (dictionary.ContainsKey(ModStrings.BACKGROUND))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.BACKGROUND]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Texture = contentManager.Load<Texture2D>($"{filePath}");
                }

                // Scrolling
                if (dictionary.ContainsKey(ModStrings.SCROLLING))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.SCROLLING]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Scrolling = contentManager.Load<Texture2D>($"{filePath}");
                }

                // Foreground
                if (dictionary.ContainsKey(ModStrings.FOREGROUND))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.FOREGROUND]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Foreground = contentManager.Load<Texture2D>($"{filePath}");
                }

                // Size
                if (platform.Texture != null)
                {
                    platform.Width = platform.Texture.Width / 2;
                    platform.Height = platform.Texture.Height;
                }
                else if (platform.Foreground != null)
                {
                    platform.Width = platform.Foreground.Width / 2;
                    platform.Height = platform.Foreground.Height;
                }
                else
                {
                    continue;
                }

                // Position
                Vector2? position = Xml.GetVector2(xmlPlatform[dictionary[ModStrings.POSITION]]);
                if (!position.HasValue)
                {
                    continue;
                }
                platform.Position = position.Value;

                // Start state
                string stateInnerText = xmlPlatform[dictionary[ModStrings.START_STATE]].InnerText.ToLower();
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

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}
