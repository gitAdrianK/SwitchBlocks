namespace SwitchBlocks.Util
{
    using System;

    public struct Animation
    {
        public const double HALF_PI = Math.PI / 2.0d;

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

        public Style AnimStyle { get; set; }
        public Curve AnimCurve { get; set; }
    }
}
