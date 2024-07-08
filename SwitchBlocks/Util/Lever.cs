using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Represent a lever with a texture, and position.<br />
    /// The lever texture contains both textures for on and off with the off on the left and on on the right.
    /// </summary>
    public struct Lever
    {
        public Texture2D Texture { get; private set; }
        public Vector2 Position { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of levers as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>A dictionary containing lists of levers with the screennumber they appear on as key</returns>
        public static Dictionary<int, List<Lever>> GetLeversDictonary(string subfolder)
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}{ModStrings.FOLDER}{sep}{ModStrings.LEVERS}{sep}{subfolder}{sep}";

            if (!Directory.Exists(path))
            {
                return null;
            }
            string[] files = Directory.GetFiles(path);
            if (files.Length == 0)
            {
                return null;
            }

            Dictionary<int, List<Lever>> dictionary = new Dictionary<int, List<Lever>>();
            Regex regex = new Regex(@"^levers(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");
            foreach (string xmlFilePath in files)
            {
                string xmlFile = xmlFilePath.Split(sep).Last();

                if (!regex.IsMatch(xmlFile))
                {
                    continue;
                }

                XmlDocument document = new XmlDocument();
                document.Load(xmlFilePath);
                XmlNode xmlLevers = document.LastChild;

                if (xmlLevers.Name != ModStrings.XML_LEVERS)
                {
                    continue;
                }

                List<Lever> lever = GetLeverList(xmlLevers, path, sep);
                if (lever.Count != 0)
                {
                    dictionary.Add(int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1, lever);
                }

            }
            return dictionary.Count == 0 ? null : dictionary;
        }

        /// <summary>
        /// Gets a list containing levers, this list may be empty.
        /// </summary>
        /// <param name="xmlLevers">The levers xml node</param>
        /// <param name="path">The path to the file</param>
        /// <param name="sep">Path separator</param>
        /// <returns>A list containing all successfully created levers</returns>
        private static List<Lever> GetLeverList(XmlNode xmlLevers, string path, char sep)
        {
            List<Lever> list = new List<Lever>();
            foreach (XmlElement xmlElement in xmlLevers.ChildNodes)
            {
                XmlNodeList xmlLever = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary = Xml.MapNamesRequired(xmlLever, ModStrings.TEXTURE, ModStrings.POSITION);
                if (dictionary == null)
                {
                    continue;
                }

                Lever lever = new Lever();
                // Texture
                string filePath = $"{path}{ModStrings.TEXTURES}{sep}{xmlLever[dictionary[ModStrings.TEXTURE]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                lever.Texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");
                lever.Width = lever.Texture.Width / 2;
                lever.Height = lever.Texture.Height;

                // Position
                Vector2? position = Xml.GetVector2(xmlLever[dictionary[ModStrings.POSITION]]);
                if (position == null)
                {
                    continue;
                }
                lever.Position = (Vector2)position;

                // The lever had all elements properly set.
                list.Add(lever);
            }
            return list;
        }
    }
}
