namespace SwitchBlocks.Util
{
    using System.Xml.Linq;

    /// <summary>
    ///     A class containing some XML helpers.
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        ///     Ways an element can be added to an XML Element.
        /// </summary>
        public enum AddAs
        {
            Parent,
            Element,
            Comment,
        }

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

        /// <summary>
        ///     Adds either an <see cref="XElement" /> or <see cref="XComment" /> to the target depending on if
        ///     the source contained the asked for element and if that element is a parent to other elements or
        ///     a "leaf" element.
        /// </summary>
        /// <param name="targetParent">The XElement to add to.</param>
        /// <param name="sourceParent">The XElement to take from.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="defaultValue">Default value should a comment be added.</param>
        /// <param name="addAs">If the element should be added as parent, element, or comment.</param>
        /// <returns><see cref="XElement" /> for further adding to, or <c>null</c> if the element is a leaf.</returns>
        public static XElement AddElementOrComment(
            XElement targetParent,
            XElement sourceParent,
            string elementName,
            string defaultValue = null,
            AddAs addAs = AddAs.Element)
        {
            if (addAs == AddAs.Parent)
            {
                var parentElement = new XElement(elementName);
                targetParent.Add(parentElement);
                return parentElement;
            }

            var element = sourceParent?.Element(elementName);
            if (element != null)
            {
                targetParent.Add(element);
                return null;
            }

            if (addAs == AddAs.Comment)
            {
                targetParent.Add(defaultValue == null
                    ? new XComment($" <{elementName} /> ")
                    : new XComment($" <{elementName}>{defaultValue}</{elementName}> "));
            }
            else
            {
                targetParent.Add(defaultValue == null
                    ? new XElement(elementName)
                    : new XElement(elementName, defaultValue));
            }

            return null;
        }

        /// <summary>
        ///     Adds either an <see cref="XElement" /> or <see cref="XComment" /> to the target if that element
        ///     is a parent to other elements or a "leaf" element.
        /// </summary>
        /// <param name="targetParent">The XElement to add to.</param>
        /// <param name="elementName">Name of the element.</param>
        /// <param name="defaultValue">Default value should a comment be added.</param>
        /// <param name="addAs">If the element should be added as parent, element, or comment.</param>
        /// <returns><see cref="XElement" /> for further adding to, or <c>null</c> if the element is a leaf.</returns>
        public static XElement AddElementOrComment(
            XElement targetParent,
            string elementName,
            string defaultValue = null,
            AddAs addAs = AddAs.Element) =>
            AddElementOrComment(targetParent, null, elementName, defaultValue, addAs);
    }
}
