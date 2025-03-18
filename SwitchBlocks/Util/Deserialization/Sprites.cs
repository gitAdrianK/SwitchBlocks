namespace SwitchBlocks.Util.Deserialization
{
    using System.Collections.Generic;
    using Microsoft.Xna.Framework;

    public class Sprites
    {
        public Point Cells { get; set; }

        public int FPS { get; set; }

        public List<float> Frames { get; set; }

        public bool RandomOffset { get; set; }

        public bool ResetWithLever { get; set; }
    }
}
