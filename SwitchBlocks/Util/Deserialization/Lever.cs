namespace SwitchBlocks.Util.Deserialization
{
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Entities;

    /// <summary>
    /// Deserialization helper to create <see cref="EntityDrawLever"/>.
    /// </summary>
    public class Lever
    {
        /// <summary><see cref="Texture2D"/>.</summary>
        public Texture2D Texture { get; set; }
        /// <summary>Position.</summary>
        public Vector2 Position { get; set; }
    }
}
