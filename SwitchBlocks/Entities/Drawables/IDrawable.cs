using Microsoft.Xna.Framework.Graphics;

namespace SwitchBlocks.Entities.Drawables
{
    public interface IDrawable
    {
        void Draw(SpriteBatch spriteBatch, bool state, float progress);
    }
}
