using System.Xml.Serialization;

namespace SwitchBlocks.Util
{
    public enum Style
    {
        [XmlEnum(Name = "")]
        None = 0,
        [XmlEnum(Name = "Fade")]
        Fade,
        [XmlEnum(Name = "Top")]
        Top,
        [XmlEnum(Name = "Bottom")]
        Bottom,
        [XmlEnum(Name = "Left")]
        Left,
        [XmlEnum(Name = "Right")]
        Right,
    }

    public enum Curve
    {
        [XmlEnum(Name = "")]
        None = 0,
        [XmlEnum(Name = "Linear")]
        Linear,
        [XmlEnum(Name = "EaseIn")]
        EaseIn,
        [XmlEnum(Name = "EaseOut")]
        EaseOut,
        [XmlEnum(Name = "EaseInOut")]
        EaseInOut,
    }

    public struct Animation
    {
        public Style Style;
        public Curve Curve;
    }
}
