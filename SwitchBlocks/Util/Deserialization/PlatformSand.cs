namespace SwitchBlocks.Util.Deserialization
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Used as a XML deserialization helper
    /// </summary>
    public class PlatformSand
    {
        public Texture2D Background { get; set; }

        public Texture2D Scrolling { get; set; }

        public Texture2D Foreground { get; set; }

        public Vector2 Position { get; set; }

        public bool StartState { get; set; }

    }
}

