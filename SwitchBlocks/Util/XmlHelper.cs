namespace SwitchBlocks.Util
{
    using System.Xml.Linq;

    /// <summary>
    ///     A class containing some XML helpers.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        ///     Tries to get an element. if the <see cref="XElement" /> is self-closing count it as <c>true</c>, otherwise try
        ///     parsing
        ///     the elements value to <c>bool</c>.
        /// </summary>
        /// <param name="root">Root element to check if the asked for element exists on.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <returns>
        ///     <c>true</c> if the element is self-closing or its content can be parsed to <c>true</c>, <c>false</c>
        ///     otherwise.
        /// </returns>
        public static bool ParseElementBool(XElement root, string elementName)
        {
            var element = root?.Element(elementName);
            if (element == null)
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(element.Value))
            {
                return true;
            }

            return bool.TryParse(element.Value, out var result) && result;
        }
    }
}
