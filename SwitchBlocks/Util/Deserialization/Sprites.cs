namespace SwitchBlocks.Util.Deserialization
{
    using Microsoft.Xna.Framework;

    public class Sprites
    {
        public Point Cells { get; set; }

        public float FPS { get; set; }

        public float[] Frames { get; set; }

        public bool RandomOffset { get; set; }

        public bool ResetWithLever { get; set; }
    }
}
