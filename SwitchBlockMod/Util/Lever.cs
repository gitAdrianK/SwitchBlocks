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
    /// Represent a lever with a texture, and position.<br />
    /// The lever texture contains both textures for on and off with the off on the left and on on the right.
    /// </summary>
    public struct Lever
    {
        public Texture2D texture;
        public Vector2 position;

        /// <summary>
        /// Creates a dictionary containing the screen as key and a list of levers as value.<br />
        /// It should be noted that the screens xml start counting at 1 while the ingame screens start at 0.
        /// </summary>
        /// <param name="subfolder">The subfolder to look for xml in. The main path is the path to the mod folder.</param>
        /// <returns>Dictionary&lt;int,List&lt;Lever&gt;&gt;</returns>
        public static Dictionary<int, List<Lever>> GetLeversDictonary(string subfolder)
        {
            JKContentManager contentManager = Game1.instance.contentManager;
            char sep = Path.DirectorySeparatorChar;
            string path = $"{contentManager.root}{sep}switchBlocksMod{sep}levers{sep}{subfolder}{sep}";
            Dictionary<int, List<Lever>> dictionary = new Dictionary<int, List<Lever>>();

            if (!Directory.Exists(path))
            {
                return dictionary;
            }

            Regex regex = new Regex(@"^levers(?:[1-9]|[1-9][0-9]|1[0-6][0-9]).xml$");

            foreach (string xmlFilePath in Directory.GetFiles(path))
            {
                string xmlFile = xmlFilePath.Split(sep).Last();

                if (!regex.IsMatch(xmlFile))
                {
                    continue;
                }

                XmlDocument document = new XmlDocument();
                document.Load(xmlFilePath);
                XmlNode xmlLevers = document.LastChild;

                if (xmlLevers.Name != "Levers")
                {
                    return dictionary;
                }

                List<Lever> lever = GetLeverList(xmlLevers, path, sep);
                if (lever.Count != 0)
                {
                    dictionary.Add(int.Parse(Regex.Replace(xmlFile, @"[^\d]", "")) - 1, lever);
                }

            }
            return dictionary;
        }

        /// <summary>
        /// Gets a list containing levers, this list may be empty.
        /// </summary>
        /// <param name="xmlLevers">The levers xml node</param>
        /// <param name="path">The path to the file</param>
        /// <param name="sep">Path separator</param>
        /// <returns>List&lt;Lever&gt;</returns>
        private static List<Lever> GetLeverList(XmlNode xmlLevers, string path, char sep)
        {
            List<Lever> list = new List<Lever>();
            foreach (XmlElement xmlElement in xmlLevers.ChildNodes)
            {
                XmlNodeList xmlLever = xmlElement.ChildNodes;
                Dictionary<string, int> dictionary = Xml.MapNamesExact(xmlLever, "Texture", "Position");
                if (dictionary == null)
                {
                    continue;
                }

                Lever lever;

                // CONSIDER: Using a stiched texture, and just saving a Rectangle w/ start xy, width, height, would probably be more performant.
                // Implement if needed.
                // Texture
                string filePath = $"{path}textures{sep}{xmlLever[dictionary["Texture"]].InnerText}";
                if (!File.Exists($"{filePath}.xnb"))
                {
                    continue;
                }
                lever.texture = Game1.instance.contentManager.Load<Texture2D>($"{filePath}");

                // Position
                Vector2? position = Xml.GetVector2(xmlLever[dictionary["Position"]]);
                if (position == null)
                {
                    continue;
                }
                lever.position = (Vector2)position;

                // The lever had all elements properly set.
                list.Add(lever);
            }
            return list;
        }
    }
}
