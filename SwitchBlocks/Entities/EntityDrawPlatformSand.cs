namespace SwitchBlocks.Entities
{
    using System;
    using Data;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util;
    using Util.Deserialization;

    public class EntityDrawPlatformSand : EntityDraw
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="PlatformScrolling" />.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        public EntityDrawPlatformSand(
            PlatformScrolling platform,
            int screen,
            IDataProvider data)
            : base(platform.Background, platform.Position, screen)
        {
            this.Scrolling = platform.Scrolling;
            this.Foreground = platform.Foreground;
            this.StartState = platform.StartState;
            this.Multiplier = platform.Multiplier;

            if (this.Texture != null)
            {
                this.Width = this.Texture.Width;
                this.Height = this.Texture.Height;
            }
            else if (this.Foreground != null)
            {
                this.Width = this.Foreground.Width;
                this.Height = this.Foreground.Height;
            }

            this.Data = data;
        }

        /// <summary>Scrolling <see cref="Texture2D" />.</summary>
        private Texture2D Scrolling { get; }

        /// <summary>Foreground <see cref="Texture2D" />.</summary>
        private Texture2D Foreground { get; }

        /// <summary>Start state.</summary>
        private StartState StartState { get; }

        /// <summary>Scroll speed multiplier.</summary>
        private float Multiplier { get; }

        /// <summary><see cref="IDataProvider" />.</summary>
        private IDataProvider Data { get; }

        /// <summary>
        ///     Draws the entity if the current screen is the screen it appears on or the game has not finished yet.
        ///     Draws background, scrolling and foreground <see cref="Texture2D" />s if not null.
        /// </summary>
        public override void Draw()
        {
            if (this.DrawGuard() || this.StartState == StartState.On != this.Data.State)
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
        ///     Draws a given <see cref="Texture2D" />.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D" />.</param>
        private void DrawTexture(Texture2D texture)
            => Game1.spriteBatch.Draw(
                texture,
                this.Position,
                new Rectangle(
                    0,
                    0,
                    this.Width,
                    this.Height),
                Color.White);

        /// <summary>
        ///     Draws the scrolling <see cref="Texture2D" /> wrapped based on progress.
        /// </summary>
        private void DrawScrolling()
        {
            var textureHeight = this.Scrolling.Height;
            var progress = this.Data.ProgressUnclamped * this.Multiplier;
            progress %= textureHeight;
            if (progress < 0)
            {
                progress += textureHeight;
            }

            progress = textureHeight - progress;


            // How much we can draw before hitting the bottom of the texture
            var viewHeight = this.Height;
            var offset = (int)progress;
            var firstPartHeight = Math.Min(textureHeight - offset, viewHeight);

            // First slice
            Game1.spriteBatch.Draw(
                this.Scrolling,
                this.Position,
                new Rectangle(0, offset, this.Width, firstPartHeight),
                Color.White);

            // Second slice (wrap to top of texture)
            if (firstPartHeight < viewHeight)
            {
                Game1.spriteBatch.Draw(
                    this.Scrolling,
                    new Vector2(this.Position.X, this.Position.Y + firstPartHeight),
                    new Rectangle(0, 0, this.Width, viewHeight - firstPartHeight),
                    Color.White);
            }
        }
    }
}
