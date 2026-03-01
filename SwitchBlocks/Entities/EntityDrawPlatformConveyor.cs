namespace SwitchBlocks.Entities
{
    using System;
    using Data;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using Util;
    using Util.Deserialization;

    public class EntityDrawPlatformConveyor : EntityDraw
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="PlatformScrolling" />.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider" />.</param>
        public EntityDrawPlatformConveyor(
            PlatformScrolling platform,
            int screen,
            IDataProvider data)
            : base(platform.Background, platform.Position, screen, platform.IsForeground)
        {
            this.Scrolling = platform.Scrolling;
            this.Foreground = platform.Foreground;
            this.StartState = platform.StartState;
            this.Multiplier = platform.Multiplier;

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
            if (this.DrawGuard())
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

        // XXX: This DESPERATELY needs to use the same draw as vanilla sand.
        // Calling .End() and .Start() repeatedly to set PointWrap is NOT an option.

        /// <summary>
        ///     Draws a given <see cref="Texture2D" />.
        /// </summary>
        /// <param name="texture"><see cref="Texture2D" />.</param>
        private void DrawTexture(Texture2D texture)
            => Game1.spriteBatch.Draw(
                texture,
                this.Position,
                new Rectangle(
                    this.Width * Convert.ToInt32(this.StartState == StartState.On != this.Data.State),
                    0,
                    this.Width,
                    this.Height),
                Color.White);

        /// <summary>
        ///     Draws the scrolling <see cref="Texture2D" /> wrapped based on progress.
        /// </summary>
        private void DrawScrolling()
        {
            var actualOffset = (int)(this.Data.ProgressUnclamped * this.Multiplier) % this.Scrolling.Width;
            actualOffset = this.StartState == StartState.On == this.Data.State
                ? actualOffset
                : this.Scrolling.Width - actualOffset;

            if (actualOffset + this.Width > this.Scrolling.Width)
            {
                var diff = this.Scrolling.Width - actualOffset;
                Game1.spriteBatch.Draw(
                    this.Scrolling,
                    this.Position,
                    new Rectangle(
                        actualOffset,
                        0,
                        diff,
                        this.Height),
                    Color.White);

                Game1.spriteBatch.Draw(
                    this.Scrolling,
                    new Vector2(
                        this.Position.X + diff,
                        this.Position.Y),
                    new Rectangle(
                        0,
                        0,
                        this.Width - diff,
                        this.Height),
                    Color.White);
                return;
            }

            Game1.spriteBatch.Draw(
                this.Scrolling,
                this.Position,
                new Rectangle(
                    actualOffset,
                    0,
                    this.Width,
                    this.Height),
                Color.White);
        }
    }
}
