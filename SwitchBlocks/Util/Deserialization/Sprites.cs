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

        /// <summary>Reset the animation when a lever is activated.</summary>
        public bool ResetWithLever { get; set; }

        /// <summary>Have the animation play even when the platform is non-solid</summary>
        public bool IgnoreState { get; set; }
    }
}
