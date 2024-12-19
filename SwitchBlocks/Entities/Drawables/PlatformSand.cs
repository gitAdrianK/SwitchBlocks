using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.IO;
using System.Xml.Serialization;

namespace SwitchBlocks.Entities.Drawables
{
    public class PlatformSand : Platform
    {
        // This roundabout way with string paths seems weird, but I haven't found
        // a different way to deserialize on StackOverflow yet, skill issue.
        private Texture2D Scrolling { get; set; }
        [XmlElement("Scrolling")]
        public string ScrollingAsString { get; set; }

        private Texture2D Foreground { get; set; }
        [XmlElement("Foreground")]
        public string ForegroundAsString { get; set; }

        private int Height { get; set; }
        private int Width { get; set; }

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

        public new bool InitializeTextures(JKContentManager contentManager, string path)
        {
            if (File.Exists($"{path}{TextureAsString}.xnb"))
            {
                Texture = contentManager.Load<Texture2D>($"{path}{TextureAsString}");
            }
            if (File.Exists($"{path}{ScrollingAsString}.xnb"))
            {
                Scrolling = contentManager.Load<Texture2D>($"{path}{ScrollingAsString}");
            }
            if (File.Exists($"{path}{ForegroundAsString}.xnb"))
            {
                Foreground = contentManager.Load<Texture2D>($"{path}{ForegroundAsString}");
            }

            return Texture != null || Foreground != null;
        }

        public new bool InitializeOthers()
        {
            if (Texture != null)
            {
                Width = Texture.Width;
                Height = Texture.Height;
                return true;
            }
            if (Foreground != null)
            {
                Width = Foreground.Width;
                Height = Foreground.Height;
                return true;
            }
            return false;
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
