namespace SwitchBlocks.Util.Deserialization
{
    using Entities;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    ///     Deserialization helper to create <see cref="EntityDrawPlatformSand" />.
    /// </summary>
    public class PlatformSand
    {
        /// <summary>Background <see cref="Texture2D" />.</summary>
        public Texture2D Background { get; set; }

        /// <summary>Scrolling <see cref="Texture2D" />.</summary>
        public Texture2D Scrolling { get; set; }

        /// <summary>Foreground <see cref="Texture2D" />.</summary>
        public Texture2D Foreground { get; set; }

        /// <summary>Position.</summary>
        public Vector2 Position { get; set; }

        /// <summary>Start state.</summary>
        public StartState StartState { get; set; }
    }
}
