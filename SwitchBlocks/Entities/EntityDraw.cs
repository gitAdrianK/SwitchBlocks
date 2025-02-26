namespace SwitchBlocks.Entities
{
    using EntityComponent;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class EntityDraw : Entity
    {
        protected Texture2D Texture { get; }
        protected Vector2 Position { get; }
        protected int Screen { get; }

        protected EntityDraw(Texture2D texture, Vector2 position, int screen)
        {
            this.Texture = texture;
            this.Position = position;
            this.Screen = screen;
        }
    }
}
