using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SwitchBlocks.Entities.Drawables
{
    public class Lever : Drawable
    {
        public override void Draw(SpriteBatch spriteBatch, bool state, float progress)
        {
            Rectangle rectangle = new Rectangle(
                    Texture.Width * (1 - Convert.ToInt32(state)) * (1 / 2),
                    0,
                    Texture.Width * (1 / 2),
                    Texture.Height);
            spriteBatch.Draw(
                texture: Texture,
                position: Position,
                sourceRectangle: rectangle,
                color: Color.White);
        }
    }
}
