using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace SwitchBlocks.Entities.Drawables
{
    public abstract class Drawable : IDrawable
    {
        public Texture2D Texture { get; protected set; }
        public Vector2 Position { get; protected set; }
        public bool StartState { get; protected set; }

        public abstract void Draw(SpriteBatch spriteBatch, bool state, float progress);
    }
}
