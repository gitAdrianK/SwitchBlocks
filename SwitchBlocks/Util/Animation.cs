namespace SwitchBlocks.Util
{
    using System.Xml.Serialization;

    public enum Style
    {
        [XmlEnum("")]
        None = 0,
        [XmlEnum("fade")]
        Fade,
        [XmlEnum("top")]
        Top,
        [XmlEnum("bottom")]
        Bottom,
        [XmlEnum("left")]
        Left,
        [XmlEnum("right")]
        Right,
    }

    public enum Curve
    {
        [XmlEnum("")]
        None = 0,
        [XmlEnum("linear")]
        Linear,
        [XmlEnum("easeIn")]
        EaseIn,
        [XmlEnum("easeOut")]
        EaseOut,
        [XmlEnum("easeInOut")]
        EaseInOut,
    }

    public struct Animation
    {
        public Style Style { get; set; }
        public Curve Curve { get; set; }
    }
}
