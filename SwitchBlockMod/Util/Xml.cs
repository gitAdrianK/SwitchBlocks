using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace SwitchBlocksMod
{
    /// <summary>
    /// Contains various functions related to working with xml nodes.
    /// </summary>
    static class Xml
    {
        /// <summary>
        /// Maps names to their position in an xml node list.
        /// </summary>
        /// <param name="nodeList">The xml node list to check.</param>
        /// <param name="names">The names to check for.</param>
        /// <returns>A dictionary mapping the name to its position in the node list.</returns>
        public static Dictionary<string, int> MapNames(XmlNodeList nodeList, params string[] names)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode node = nodeList[i];
                if (!names.Contains(node.Name))
                {
                    continue;
                }
                dictionary[node.Name] = i;
            }
            return dictionary;
        }

        /// <summary>
        /// Maps names to their position in an xml node list.<br />
        /// The names must be in the nodes list, and the nodes list must only contain those names.
        /// </summary>
        /// <param name="nodeList">The xml node list to check.</param>
        /// <param name="names">The names to check for.</param>
        /// <returns>A dictionary mapping the name to its position in the node list, null otherwise.</returns>
        public static Dictionary<string, int> MapNamesExact(XmlNodeList nodeList, params string[] names)
        {
            if (nodeList.Count != names.Length)
            {
                return null;
            }
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode node = nodeList[i];
                if (!names.Contains(node.Name))
                {
                    return null;
                }
                dictionary[node.Name] = i;
            }
            return dictionary;
        }

        /// <summary>
        /// Creates a Color from a given xml node should the node contain 3
        /// children named "R", "G", and "B"
        /// </summary>
        /// <param name="root">Xml node to create the color from.</param>
        /// <returns>Color or null</returns>
        public static Color? GetColor(XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            Dictionary<string, int> dictionary = MapNamesExact(children, "R", "G", "B");
            if (dictionary == null)
            {
                return null;
            }
            return new Color(
                int.Parse(children[dictionary["R"]].InnerText),
                int.Parse(children[dictionary["G"]].InnerText),
                int.Parse(children[dictionary["B"]].InnerText));
        }

        /// <summary>
        /// Creates a Vector2 from a given xml node should the node contain 2
        /// children named "X", and "Y"
        /// </summary>
        /// <param name="root">Xml node to create the vector from.</param>
        /// <returns>Vector2 or null</returns>
        public static Vector2? GetVector2(XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            Dictionary<string, int> dictionary = MapNamesExact(children, "X", "Y");
            if (dictionary == null)
            {
                return null;
            }
            return new Vector2(
                int.Parse(children[dictionary["X"]].InnerText),
                int.Parse(children[dictionary["Y"]].InnerText));
        }
    }
}
