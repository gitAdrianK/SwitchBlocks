﻿using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Setups;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Diagnostics;
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

                int screenNr = int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1;
                List<PlatformGroup> platforms = GetPlatformList(xmlPlatforms, path, sep, screenNr);
                if (platforms.Count != 0)
                {
                    dictionary.Add(screenNr, platforms);
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

                PlatformGroup platform = new PlatformGroup();
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

                // Link
                Vector3 link = new Vector3(
                    (int)(platform.Position.X / 8),
                    (int)(platform.Position.Y / 8),
                    screenNr);
                if (dictionary.ContainsKey(ModStrings.LINK_POSITION))
                {
                    Vector3? optionalLink = Xml.GetVector3(xmlPlatform[dictionary[ModStrings.LINK_POSITION]]);
                    if (optionalLink == null)
                    {
                        continue;
                    }
                    link = (Vector3)optionalLink;
                }
                Debugger.Log(1, "", ">>> Link position " + link + "\n");
                if (SetupGroup.BlocksGroupA.ContainsKey(link))
                {
                    platform.GroupId = SetupGroup.BlocksGroupA[link].GroupId;
                }
                else if (SetupGroup.BlocksGroupB.ContainsKey(link))
                {
                    platform.GroupId = SetupGroup.BlocksGroupB[link].GroupId;
                }
                else
                {
                    Debugger.Log(1, "", ">>> Found no block at link position\n");
                    continue;
                }
                Debugger.Log(1, "", ">>> Found block, setting Group ID to " + platform.GroupId + "\n");

                // Start state
                platform.StartState = false;

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
