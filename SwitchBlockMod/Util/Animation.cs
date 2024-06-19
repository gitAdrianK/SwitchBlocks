using System;

namespace SwitchBlocksMod.Util
{
    public struct Animation
    {
        public const double HALF_PI = Math.PI / 2.0d;

        public enum Style
        {
            Fade,
            Top,
            Bottom,
            Left,
            Right,
        }

        public enum Curve
        {
            Stepped,
            Linear,
            EaseIn,
            EaseOut,
            EaseInOut,
        }

        public Style style;
        public Curve curve;
    }
}
