namespace SwitchBlocks.Util
{
    public enum Style : byte
    {
        Fade,
        Top,
        Bottom,
        Left,
        Right,
    }

    public enum Curve : byte
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        Stepped,
    }

    public struct Animation
    {
        public Style Style { get; set; }
        public Curve Curve { get; set; }
    }
}
