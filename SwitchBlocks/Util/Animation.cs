namespace SwitchBlocks.Util
{
    public enum Style
    {
        None = 0,
        Fade,
        Top,
        Bottom,
        Left,
        Right,
    }

    public enum Curve
    {
        None = 0,
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
    }

    public struct Animation
    {
        public Style Style { get; set; }
        public Curve Curve { get; set; }
    }
}
