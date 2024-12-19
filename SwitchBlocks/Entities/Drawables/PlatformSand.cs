namespace SwitchBlocks.Entities.Drawables
{
    using System;
    using System.IO;
    using System.Xml.Serialization;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;

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
            if (this.Texture != null)
            {
                this.DrawBackground(spriteBatch, state);
            }

            if (this.Scrolling != null)
            {
                this.DrawScrolling(spriteBatch, state, progress);
            }

            if (this.Foreground != null)
            {
                this.DrawForeground(spriteBatch, state);
            }
        }

        public new bool InitializeTextures(JKContentManager contentManager, string path)
        {
            if (File.Exists($"{path}{this.TextureAsString}.xnb"))
            {
                this.Texture = contentManager.Load<Texture2D>($"{path}{this.TextureAsString}");
            }
            if (File.Exists($"{path}{this.ScrollingAsString}.xnb"))
            {
                this.Scrolling = contentManager.Load<Texture2D>($"{path}{this.ScrollingAsString}");
            }
            if (File.Exists($"{path}{this.ForegroundAsString}.xnb"))
            {
                this.Foreground = contentManager.Load<Texture2D>($"{path}{this.ForegroundAsString}");
            }

            return this.Texture != null || this.Foreground != null;
        }

        public override bool InitializeOthers()
        {
            if (this.Texture != null)
            {
                this.Width = this.Texture.Width;
                this.Height = this.Texture.Height;
                return true;
            }
            if (this.Foreground != null)
            {
                this.Width = this.Foreground.Width;
                this.Height = this.Foreground.Height;
                return true;
            }
            return false;
        }

        private void DrawBackground(SpriteBatch spriteBatch, bool state) => this.DrawTexture(spriteBatch, state, this.Texture);

        private void DrawForeground(SpriteBatch spriteBatch, bool state) => this.DrawTexture(spriteBatch, state, this.Foreground);

        private void DrawTexture(SpriteBatch spriteBatch, bool state, Texture2D texture)
        {
            var sourceRectangle = new Rectangle(
                    this.Width * (1 - Convert.ToInt32(this.StartState == state)),
                    0,
                    this.Width,
                    this.Height);

            spriteBatch.Draw(
                texture: texture,
                position: this.Position,
                sourceRectangle: sourceRectangle,
                color: Color.White);
        }

        private void DrawScrolling(SpriteBatch spriteBatch, bool state, float progress)
        {
            var actualOffset = (int)(progress % this.Scrolling.Height);
            actualOffset = this.StartState == state ? actualOffset : this.Scrolling.Height - actualOffset;

            // Depending on if the offset would make it so we go past the texture.
            if (actualOffset + this.Height > this.Scrolling.Height)
            {
                var diff = this.Scrolling.Height - actualOffset;

                spriteBatch.Draw(
                texture: this.Scrolling,
                position: this.Position,
                sourceRectangle: new Rectangle(
                    0,
                    actualOffset,
                    this.Width,
                    diff),
                color: Color.White);

                spriteBatch.Draw(
                texture: this.Scrolling,
                position: new Vector2(
                    this.Position.X,
                    this.Position.Y + diff),
                sourceRectangle: new Rectangle(
                    0,
                    0,
                    this.Width,
                    this.Height - diff),
                color: Color.White);
                return;
            }
            spriteBatch.Draw(
                texture: this.Scrolling,
                position: this.Position,
                sourceRectangle: new Rectangle(
                    0,
                    actualOffset,
                    this.Width,
                    this.Height),
                color: Color.White);
        }
    }
}
