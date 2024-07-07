using Microsoft.Xna.Framework;
using SwitchBlocksMod.Util;
using System.Collections.Generic;
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
        /// <returns>A dictionary mapping the names to its position in the node list.</returns>
        public static Dictionary<string, int> MapNames(XmlNodeList nodeList)
        {
            Dictionary<string, int> dictionary = new Dictionary<string, int>();
            for (int i = 0; i < nodeList.Count; i++)
            {
                XmlNode node = nodeList[i];
                dictionary[node.Name] = i;
            }
            return dictionary;
        }

        /// <summary>
        /// Maps names to their position in an xml node list.<br />
        /// The names must be in the nodes list, and the nodes list must at least contain those names.
        /// </summary>
        /// <param name="nodeList">The xml node list to check.</param>
        /// <param name="names">The names to check for.</param>
        /// <returns>A dictionary mapping the names to its position in the node list, null otherwise.</returns>
        public static Dictionary<string, int> MapNamesRequired(XmlNodeList nodeList, params string[] names)
        {
            if (nodeList.Count < names.Length)
            {
                return null;
            }
            Dictionary<string, int> dictionary = MapNames(nodeList);
            foreach (string name in names)
            {
                if (!dictionary.ContainsKey(name))
                {
                    return null;
                }
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
            Dictionary<string, int> dictionary = MapNamesRequired(children, "R", "G", "B");
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
            Dictionary<string, int> dictionary = MapNamesRequired(children, "X", "Y");
            if (dictionary == null)
            {
                return null;
            }
            return new Vector2(
                float.Parse(children[dictionary["X"]].InnerText),
                float.Parse(children[dictionary["Y"]].InnerText));
        }

        /// <summary>
        /// Creates a Point from a given xml node should the node contain 2
        /// children named "X", and "Y"
        /// </summary>
        /// <param name="root">Xml node to create the point from.</param>
        /// <returns>Point or null</returns>
        public static Point? GetPoint(XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            Dictionary<string, int> dictionary = MapNamesRequired(children, "X", "Y");
            if (dictionary == null)
            {
                return null;
            }
            return new Point(
                int.Parse(children[dictionary["X"]].InnerText),
                int.Parse(children[dictionary["Y"]].InnerText));
        }

        /// <summary>
        /// Creates an Animation from a given xml node .
        /// </summary>
        /// <param name="root">Xml node to create the animation from.</param>
        /// <returns>Animation</returns>
        public static Animation GetAnimation(XmlNode root)
        {
            XmlNodeList children = root.ChildNodes;
            Animation animation;
            animation.style = Animation.Style.Fade;
            animation.curve = Animation.Curve.Linear;
            Dictionary<string, int> dictionary = MapNames(children);
            if (dictionary.ContainsKey("Style"))
            {
                switch (children[dictionary["Style"]].InnerText)
                {
                    case "fade":
                        animation.style = Animation.Style.Fade;
                        break;
                    case "top":
                        animation.style = Animation.Style.Top;
                        break;
                    case "bottom":
                        animation.style = Animation.Style.Bottom;
                        break;
                    case "left":
                        animation.style = Animation.Style.Left;
                        break;
                    case "right":
                        animation.style = Animation.Style.Right;
                        break;
                    default:
                        animation.style = Animation.Style.Fade;
                        break;
                }
            }
            if (dictionary.ContainsKey("Curve"))
            {
                switch (children[dictionary["Curve"]].InnerText)
                {
                    case "stepped":
                        animation.curve = Animation.Curve.Stepped;
                        break;
                    case "linear":
                        animation.curve = Animation.Curve.Linear;
                        break;
                    case "easeIn":
                        animation.curve = Animation.Curve.EaseIn;
                        break;
                    case "easeOut":
                        animation.curve = Animation.Curve.EaseOut;
                        break;
                    case "easeInOut":
                        animation.curve = Animation.Curve.EaseInOut;
                        break;
                    default:
                        animation.curve = Animation.Curve.Linear;
                        break;
                }
            }
            return animation;
        }
    }
}
