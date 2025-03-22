namespace SwitchBlocks.Entities
{
    using EntityComponent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    /// <summary>
    /// Abstract class other drawing entities inherit from.
    /// </summary>
    public abstract class EntityDraw : Entity
    {
        /// <summary><see cref="Texture2D"/>.</summary>
        protected Texture2D Texture { get; }
        /// <summary>Position.</summary>
        protected Vector2 Position { get; }
        /// <summary>Height.</summary>
        protected int Height { get; set; }
        /// <summary>Width.</summary>
        protected int Width { get; set; }
        /// <summary>Screen this entity is on.</summary>
        protected int Screen { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/>.</param>
        /// <param name="position">Position.</param>
        /// <param name="screen">Screen this entity is on.</param>
        protected EntityDraw(Texture2D texture, Vector2 position, int screen)
        {
            this.Texture = texture;
            this.Height = texture.Height;
            this.Width = texture.Width;
            this.Position = position;
            this.Screen = screen;
        }
    }
}
