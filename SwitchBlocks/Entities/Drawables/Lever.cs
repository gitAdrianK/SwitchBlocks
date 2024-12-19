namespace SwitchBlocks.Entities.Drawables
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

    public class Lever : Drawable
    {
        public override void Draw(SpriteBatch spriteBatch, bool state, float progress)
        {
            var rectangle = new Rectangle(
                    this.Texture.Width / 2 * (1 - Convert.ToInt32(state)),
                    0,
                    this.Texture.Width / 2,
                    this.Texture.Height);
            spriteBatch.Draw(
                texture: this.Texture,
                position: this.Position,
                sourceRectangle: rectangle,
                color: Color.White);
        }
    }
}
