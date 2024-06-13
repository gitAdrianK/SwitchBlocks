﻿using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

// Don't like the code duplication but w/e

namespace SwitchBlocksMod.Util
{
    /// <summary>
    /// Represents a subclass of platform, sandplatform with three textures, background, scrolling and, foreground. <br />
    /// A foreground is optional.
    /// </summary>
    public class PlatformSand : Platform
    {
        public Texture2D background;
        public Texture2D scrolling;
        public Texture2D foreground;

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of platforms as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>A dictionary containing lists of platforms with the screennumber they appear on as key</returns>
        public static new Dictionary<int, List<Platform>> GetPlatformsDictonary(string subfolder)
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}{ModStrings.PLATFORMS}{sep}{subfolder}{sep}";
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

                if (xmlPlatforms.Name != ModStrings.XML_PLATFORMS)
                {
                    return dictionary;
                }

                List<Platform> platforms = GetPlatformList(xmlPlatforms, path, sep, subfolder);
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
        private static List<Platform> GetPlatformList(XmlNode xmlPlatforms, string path, char sep, string subfolder)
        {
            List<Platform> list = new List<Platform>();
            foreach (XmlElement xmlElement in xmlPlatforms.ChildNodes)
            {
                XmlNodeList xmlPlatform = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary;
                dictionary = Xml.MapNamesRequired(xmlPlatform,
                    ModStrings.BACKGROUND,
                    ModStrings.SCROLLING,
                    ModStrings.POSITION,
                    ModStrings.START_STATE);

                if (dictionary == null)
                {
                    continue;
                }

                PlatformSand platform = new PlatformSand();
                // Background
                string filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.BACKGROUND]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                platform.background = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");


                // Scrolling
                filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.SCROLLING]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                platform.scrolling = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");

                // Foreground
                if (dictionary.ContainsKey(ModStrings.FOREGROUND))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.FOREGROUND]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.foreground = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                }

                // Position
                Vector2? position = Xml.GetVector2(xmlPlatform[dictionary[ModStrings.POSITION]]);
                if (position == null)
                {
                    continue;
                }
                platform.position = (Vector2)position;

                // Start state
                string stateInnerText = xmlPlatform[dictionary[ModStrings.START_STATE]].InnerText;
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