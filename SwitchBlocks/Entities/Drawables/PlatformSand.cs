using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SwitchBlocks.Entities.Drawables
{
    public class PlatformSand : Drawable
    {
        public Texture2D Scrolling { get; private set; }
        public Texture2D Foreground { get; private set; }
        public int Height { get; private set; }
        public int Width { get; private set; }

        public override void Draw(SpriteBatch spriteBatch, bool state, float progress)
        {
            if (Texture != null)
            {
                DrawBackground(spriteBatch, state);
            }

            if (Scrolling != null)
            {
                DrawScrolling(spriteBatch, state, progress);
            }

            if (Foreground != null)
            {
                DrawForeground(spriteBatch, state);
            }
        }

        private void DrawBackground(SpriteBatch spriteBatch, bool state)
        {
            DrawTexture(spriteBatch, state, Texture);
        }

        private void DrawForeground(SpriteBatch spriteBatch, bool state)
        {
            DrawTexture(spriteBatch, state, Foreground);
        }

        private void DrawTexture(SpriteBatch spriteBatch, bool state, Texture2D texture)
        {
            Rectangle sourceRectangle = new Rectangle(
                    Width * (1 - Convert.ToInt32(StartState == state)),
                    0,
                    Width,
                    Height);

            spriteBatch.Draw(
                texture: texture,
                position: Position,
                sourceRectangle: sourceRectangle,
                color: Color.White);
        }

        private void DrawScrolling(SpriteBatch spriteBatch, bool state, float progress)
        {
            int actualOffset = (int)(progress % Scrolling.Height);
            actualOffset = StartState == state ? actualOffset : Scrolling.Height - actualOffset;

            // Depending on if the offset would make it so we go past the texture.
            if (actualOffset + Height > Scrolling.Height)
            {
                int diff = Scrolling.Height - actualOffset;

                spriteBatch.Draw(
                texture: Scrolling,
                position: Position,
                sourceRectangle: new Rectangle(
                    0,
                    actualOffset,
                    Width,
                    diff),
                color: Color.White);

                spriteBatch.Draw(
                texture: Scrolling,
                position: new Vector2(
                    Position.X,
                    Position.Y + diff),
                sourceRectangle: new Rectangle(
                    0,
                    0,
                    Width,
                    Height - diff),
                color: Color.White);
                return;
            }
            spriteBatch.Draw(
                texture: Scrolling,
                position: Position,
                sourceRectangle: new Rectangle(
                    0,
                    actualOffset,
                    Width,
                    Height),
                color: Color.White);
        }
    }
}
