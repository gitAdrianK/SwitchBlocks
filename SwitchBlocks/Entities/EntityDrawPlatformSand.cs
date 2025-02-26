namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;

    public class EntityDrawPlatformSand : EntityDraw
    {
        private Texture2D Scrolling { get; }
        private Texture2D Foreground { get; }
        private bool StartState { get; }
        private int Height { get; }
        private int Width { get; }
        private IDataProvider Logic { get; }

        public EntityDrawPlatformSand(
            Texture2D background,
            Texture2D scrolling,
            Texture2D foreground,
            Vector2 position,
            bool startState,
            int screen,
            IDataProvider logic)
            : base(background, position, screen)
        {
            this.Scrolling = scrolling;
            this.Foreground = foreground;
            this.StartState = startState;

            if (this.Texture != null)
            {
                this.Width = this.Texture.Width / 2;
                this.Height = this.Texture.Height;
            }
            else if (this.Foreground != null)
            {
                this.Width = this.Foreground.Width / 2;
                this.Height = this.Foreground.Height;
            }

            this.Logic = logic;
        }

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || EndingManager.HasFinished)
            {
                return;
            }

            if (this.Texture != null)
            {
                this.DrawTexture(this.Texture);
            }

            if (this.Scrolling != null)
            {
                this.DrawScrolling();
            }

            if (this.Foreground != null)
            {
                this.DrawTexture(this.Foreground);
            }
        }

        private void DrawTexture(Texture2D texture)
            => Game1.spriteBatch.Draw(
                texture: texture,
                position: this.Position,
                sourceRectangle: new Rectangle(
                    this.Width * Convert.ToInt32(this.StartState != this.Logic.State),
                    0,
                    this.Width,
                    this.Height),
                color: Color.White);

        private void DrawScrolling()
        {
            var actualOffset = (int)(this.Logic.Progress % this.Scrolling.Height);
            actualOffset = this.StartState == this.Logic.State ? actualOffset : this.Scrolling.Height - actualOffset;

            // Depending on if the offset would make it so we go past the texture.
            if (actualOffset + this.Height > this.Scrolling.Height)
            {
                var diff = this.Scrolling.Height - actualOffset;
                Game1.spriteBatch.Draw(
                    texture: this.Scrolling,
                    position: this.Position,
                    sourceRectangle: new Rectangle(
                        0,
                        actualOffset,
                        this.Width,
                        diff),
                    color: Color.White);

                Game1.spriteBatch.Draw(
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
            Game1.spriteBatch.Draw(
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
