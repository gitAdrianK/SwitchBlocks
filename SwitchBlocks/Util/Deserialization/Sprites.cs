namespace SwitchBlocks.Util.Deserialization
{
    using Entities;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     Deserialization helper to create <see cref="EntityDrawPlatformLoop" /> and <see cref="EntityDrawPlatformReset" />.
    /// </summary>
    public class Sprites
    {
        /// <summary>Amount of sprite cells.</summary>
        public Point Cells { get; set; }

        /// <summary>Frames per second.</summary>
        public float Fps { get; set; }

        /// <summary>Frames duration.</summary>
        public float[] Frames { get; set; }

        /// <summary>Random offset.</summary>
        public bool RandomOffset { get; set; }

        /// <summary>Reset with lever. Hardcoded to work properly ONLY with the Countdown type.</summary>
        public bool ResetWithLever { get; set; }
    }
}
