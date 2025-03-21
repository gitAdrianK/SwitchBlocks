namespace SwitchBlocks.Entities
{
    using EntityComponent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public abstract class EntityDraw : Entity
    {
        protected Texture2D Texture { get; }
        protected Vector2 Position { get; }
        protected int Screen { get; }
        protected int Height { get; set; }
        protected int Width { get; set; }

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
