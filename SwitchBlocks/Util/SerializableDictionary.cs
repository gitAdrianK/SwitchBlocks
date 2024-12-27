namespace SwitchBlocks.Util
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    /// <summary>
    /// Serializable dictionary taken from Pault Welter's Weblog
    /// https://asp-blogs.azurewebsites.net/pwelter34/444961
    /// </summary>
    [XmlRoot("dictionary")]
    public class SerializableDictionary<TKey, TValue>
    : Dictionary<TKey, TValue>, IXmlSerializable
    {
        #region IXmlSerializable Members
        public System.Xml.Schema.XmlSchema GetSchema() => null;

        public void ReadXml(System.Xml.XmlReader reader)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            var wasEmpty = reader.IsEmptyElement;
            _ = reader.Read();
            if (wasEmpty)
            {
                return;
            }
            while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
            {
                reader.ReadStartElement("item");
                reader.ReadStartElement("key");

                var key = (TKey)keySerializer.Deserialize(reader);

                reader.ReadEndElement();
                reader.ReadStartElement("value");

                var value = (TValue)valueSerializer.Deserialize(reader);

                reader.ReadEndElement();

                this.Add(key, value);

                reader.ReadEndElement();
                _ = reader.MoveToContent();
            }
            reader.ReadEndElement();
        }

        public void WriteXml(System.Xml.XmlWriter writer)
        {
            var keySerializer = new XmlSerializer(typeof(TKey));
            var valueSerializer = new XmlSerializer(typeof(TValue));

            foreach (var key in this.Keys)
            {
                writer.WriteStartElement("item");
                writer.WriteStartElement("key");

                keySerializer.Serialize(writer, key);

                writer.WriteEndElement();
                writer.WriteStartElement("value");

                var value = this[key];

                valueSerializer.Serialize(writer, value);

                writer.WriteEndElement();
                writer.WriteEndElement();
            }
        }
        #endregion
    }
}
