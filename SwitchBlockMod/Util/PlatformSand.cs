using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace SwitchBlocksMod.Util
{
    /// <summary>
    /// Represents a subclass of platform, sandplatform with two more textures, scrolling and foreground. <br />
    /// Having at least a background OR foreground is required.
    /// </summary>
    public class PlatformSand : Platform
    {
        public Texture2D Scrolling { get; private set; }
        public Texture2D Foreground { get; private set; }

        /// <summary>
        /// Gets a list containing platforms, this list may be empty.
        /// </summary>
        /// <param name="xmlPlatforms">The platforms xml node</param>
        /// <param name="path">The path to the file</param>
        /// <param name="sep">Path separator</param>
        /// <returns>A list containing all successfully created platforms</returns>
        protected static new List<Platform> GetPlatformList(XmlNode xmlPlatforms, string path, char sep)
        {
            List<Platform> list = new List<Platform>();
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
                // Background
                if (dictionary.ContainsKey(ModStrings.BACKGROUND))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.BACKGROUND]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                }

                // Scrolling
                if (dictionary.ContainsKey(ModStrings.SCROLLING))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.SCROLLING]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Scrolling = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                }

                // Foreground
                if (dictionary.ContainsKey(ModStrings.FOREGROUND))
                {
                    filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlPlatform[dictionary[ModStrings.FOREGROUND]].InnerText}";
                    if (!File.Exists($"{filePath}.xnb"))
                    {
                        continue;
                    }
                    platform.Foreground = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
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

                // The platform had all elements properly set.
                list.Add(platform);
            }
            return list;
        }
    }
}
