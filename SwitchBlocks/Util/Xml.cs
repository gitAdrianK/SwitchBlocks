namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Globalization;
    using System.Xml;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// Contains various functions related to working with xml nodes.
    /// </summary>
    public static class Xml
    {
        /// <summary>
        /// Maps names to their position in an xml node list.
        /// </summary>
        /// <param name="nodeList">The xml node list to check.</param>
        /// <returns>A dictionary mapping the names to its position in the node list.</returns>
        public static Dictionary<string, int> MapNames(XmlNodeList nodeList)
        {
            var dictionary = new Dictionary<string, int>();
            for (var i = 0; i < nodeList.Count; i++)
            {
                var node = nodeList[i];
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
            var dictionary = MapNames(nodeList);
            foreach (var name in names)
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
        /// <returns>Color or null if a color couldn't be made</returns>
        public static Color? GetColor(XmlNode root)
        {
            var children = root.ChildNodes;
            var dictionary = MapNamesRequired(children, "R", "G", "B");
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
        /// <returns>Vector2 or null if a vector couldn't be made</returns>
        public static Vector2? GetVector2(XmlNode root)
        {
            var children = root.ChildNodes;
            var dictionary = MapNamesRequired(children, "X", "Y");
            if (dictionary == null)
            {
                return null;
            }
            var xString = children[dictionary["X"]].InnerText;
            xString = xString.Trim();
            xString = xString.Replace(",", ".");
            var yString = children[dictionary["Y"]].InnerText;
            yString = yString.Trim();
            yString = yString.Replace(",", ".");
            return new Vector2(
                float.Parse(xString, CultureInfo.InvariantCulture),
                float.Parse(yString, CultureInfo.InvariantCulture));
        }

        /// <summary>
        /// Creates a Point from a given xml node should the node contain 2
        /// children named "X", and "Y"
        /// </summary>
        /// <param name="root">Xml node to create the point from.</param>
        /// <returns>Point or null if a point couldn't be made</returns>
        public static Point? GetPoint(XmlNode root)
        {
            var children = root.ChildNodes;
            var dictionary = MapNamesRequired(children, "X", "Y");
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
            var children = root.ChildNodes;
            var animation = new Animation
            {
                AnimStyle = Animation.Style.Fade,
                AnimCurve = Animation.Curve.Linear
            };
            var dictionary = MapNames(children);
            if (dictionary.ContainsKey("Style"))
            {
                switch (children[dictionary["Style"]].InnerText)
                {
                    case "fade":
                        animation.AnimStyle = Animation.Style.Fade;
                        break;
                    case "top":
                        animation.AnimStyle = Animation.Style.Top;
                        break;
                    case "bottom":
                        animation.AnimStyle = Animation.Style.Bottom;
                        break;
                    case "left":
                        animation.AnimStyle = Animation.Style.Left;
                        break;
                    case "right":
                        animation.AnimStyle = Animation.Style.Right;
                        break;
                    default:
                        animation.AnimStyle = Animation.Style.Fade;
                        break;
                }
            }
            if (dictionary.ContainsKey("Curve"))
            {
                switch (children[dictionary["Curve"]].InnerText)
                {
                    case "linear":
                        animation.AnimCurve = Animation.Curve.Linear;
                        break;
                    case "easeIn":
                        animation.AnimCurve = Animation.Curve.EaseIn;
                        break;
                    case "easeOut":
                        animation.AnimCurve = Animation.Curve.EaseOut;
                        break;
                    case "easeInOut":
                        animation.AnimCurve = Animation.Curve.EaseInOut;
                        break;
                    default:
                        animation.AnimCurve = Animation.Curve.Linear;
                        break;
                }
            }
            return animation;
        }

        public static int? GetLink(XmlNode root)
        {
            var children = root.ChildNodes;
            var dictionary = MapNamesRequired(children, "Screen", "X", "Y");
            if (dictionary == null)
            {
                return null;
            }
            var screen = int.Parse(children[dictionary["Screen"]].InnerText) - 1;
            var x = int.Parse(children[dictionary["X"]].InnerText) / 8;
            var y = int.Parse(children[dictionary["Y"]].InnerText) / 8;
            return (screen * 10000) + (x * 100) + y;
        }
    }
}
