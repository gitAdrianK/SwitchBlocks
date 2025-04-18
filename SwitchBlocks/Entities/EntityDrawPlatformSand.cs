namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patches;
    using SwitchBlocks.Util.Deserialization;

    public class EntityDrawPlatformSand : EntityDraw
    {
        /// <summary>Scrolling <see cref="Texture2D"/>.</summary>
        private Texture2D Scrolling { get; }
        /// <summary>Foreground <see cref="Texture2D"/>.</summary>
        private Texture2D Foreground { get; }
        /// <summary>Start state.</summary>
        private bool StartState { get; }
        /// <summary><see cref="IDataProvider"/>.</summary>
        private IDataProvider Data { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="PlatformSand"/>.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider"/>.</param>
        public EntityDrawPlatformSand(
            PlatformSand platform,
            int screen,
            IDataProvider data)
            : base(platform.Background, platform.Position, screen)
        {
            this.Scrolling = platform.Scrolling;
            this.Foreground = platform.Foreground;
            this.StartState = platform.StartState;

            if (this.Texture != null)
            {
                this.Width /= 2;
                this.Height = this.Texture.Height;
            }
            else if (this.Foreground != null)
            {
                this.Width = this.Foreground.Width / 2;
                this.Height = this.Foreground.Height;
            }

            this.Data = data;
        }

        /// <summary>
        /// Draws the entity if the current screen is the screen it appears on or the game has not finished yet.
        /// Draws background, scrolling and foreground <see cref="Texture2D"/>s if not null.
        /// </summary>
        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || PatchEndingManager.HasFinished)
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

        /// <summary>
        /// Draws a given <see cref="Texture2D"/>.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D"/>.</param>
        private void DrawTexture(Texture2D texture)
            => Game1.spriteBatch.Draw(
                texture: texture,
                position: this.Position,
                sourceRectangle: new Rectangle(
                    this.Width * Convert.ToInt32(this.StartState != this.Data.State),
                    0,
                    this.Width,
                    this.Height),
                color: Color.White);

        /// <summary>
        /// Draws the scrolling <see cref="Texture2D"/> wrapped based on progress.
        /// </summary>
        private void DrawScrolling()
        {
            var actualOffset = (int)(this.Data.Progress % this.Scrolling.Height);
            actualOffset = this.StartState == this.Data.State ? actualOffset : this.Scrolling.Height - actualOffset;

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
