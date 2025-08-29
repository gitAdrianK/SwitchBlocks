namespace SwitchBlocks.Util
{
    /// <summary><see cref="Animation" /> styles.</summary>
    public enum Style : byte
    {
        Fade,
        Top,
        Bottom,
        Left,
        Right
    }

    /// <summary><see cref="Animation" /> curves.</summary>
    public enum Curve : byte
    {
        Linear,
        EaseIn,
        EaseOut,
        EaseInOut,
        Stepped
    }

    /// <summary>An animation with an <see cref="Style" /> and <see cref="Curve" />.</summary>
    public struct Animation
    {
        public Style Style { get; set; }
        public Curve Curve { get; set; }
    }
}
