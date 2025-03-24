namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patches;
    using SwitchBlocks.Util;
    using SwitchBlocks.Util.Deserialization;
    using Curve = Util.Curve;

    /// <summary>
    /// Platform drawn based on data.
    /// </summary>
    public class EntityDrawPlatform : EntityDraw
    {
        /// <summary>Half of <see cref="Math.PI"/> used for animations.</summary>
        private const double HALF_PI = Math.PI / 2.0d;
        /// <summary>Start state.</summary>
        protected bool StartState { get; }
        /// <summary>Animation.</summary>
        protected Animation Animation { get; }
        /// <summary>Out animation.</summary>
        protected Animation AnimationOut { get; }

        /// <summary><see cref="IDataProvider"/>.</summary>
        protected IDataProvider Data { get; }

        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="platform">Deserialization helper <see cref="Platform"/>.</param>
        /// <param name="screen">Screen this entity is on.</param>
        /// <param name="data"><see cref="IDataProvider"/>.</param>
        public EntityDrawPlatform(
            Platform platform,
            int screen,
            IDataProvider data) : base(platform.Texture, platform.Position, screen)
        {
            this.StartState = platform.StartState;
            this.Animation = platform.Animation;
            this.AnimationOut = platform.AnimationOut;
            this.Data = data;
        }

        /// <summary>
        /// Draws the entity if the current screen is the screen it appears on or the game has not finished yet.
        /// </summary>
        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || PatchEndingManager.HasFinished)
            {
                return;
            }
            this.DrawWithRectangle(new Rectangle(0, 0, this.Width, this.Height));
        }

        /// <summary>
        /// Draws the entity with a given rectangle to limit the <see cref="Texture2D"/> to.
        /// </summary>
        /// <param name="rect"><see cref="Rectangle"/> to limit the texture to.</param>
        /// <exception cref="NotImplementedException">This should never happen.</exception>
        protected void DrawWithRectangle(Rectangle rect)
        {
            var progressAdjusted = this.StartState ? 1.0f - this.Data.Progress : this.Data.Progress;
            if (progressAdjusted == 0.0f)
            {
                return;
            }
            if (progressAdjusted == 1.0f)
            {
                Game1.spriteBatch.Draw(
                    texture: this.Texture,
                    position: this.Position,
                    sourceRectangle: rect,
                    color: Color.White);
                return;
            }

            float progressActual;
            var animation = this.StartState == this.Data.State ? this.Animation : this.AnimationOut;
            switch (animation.Curve)
            {
                case Curve.Linear:
                    progressActual = progressAdjusted;
                    break;
                case Curve.EaseIn:
                    progressActual = (float)Math.Sin((progressAdjusted * HALF_PI) - HALF_PI) + 1.0f;
                    break;
                case Curve.EaseOut:
                    progressActual = (float)Math.Sin(progressAdjusted * HALF_PI);
                    break;
                case Curve.EaseInOut:
                    progressActual = (float)(Math.Sin((progressAdjusted * Math.PI) - HALF_PI) + 1.0f) / 2.0f;
                    break;
                case Curve.Stepped:
                    progressActual = this.StartState == this.Data.State ? 0.0f : 1.0f;
                    break;
                default:
                    throw new NotImplementedException("Unknown Animation Curve, cannot draw!");
            }
            if (progressActual == 0.0f)
            {
                return;
            }

            var color = Color.White;
            var pos = this.Position;
            switch (animation.Style)
            {
                case Style.Fade:
                    color = new Color(
                        progressActual,
                        progressActual,
                        progressActual,
                        progressActual);
                    break;
                case Style.Top:
                    var heightTop = (int)(this.Height * progressActual);
                    rect.Y = this.Height - heightTop;
                    rect.Height = heightTop;
                    break;
                case Style.Bottom:
                    var heightBottom = (int)(this.Height * progressActual);
                    pos.Y += this.Height - heightBottom;
                    rect.Height = heightBottom;
                    break;
                case Style.Left:
                    var widthLeft = (int)(this.Width * progressActual);
                    rect.X = this.Width - widthLeft;
                    rect.Width = widthLeft;
                    break;
                case Style.Right:
                    var widthRight = (int)(this.Width * progressActual);
                    pos.X += this.Width - widthRight;
                    rect.Width = widthRight;
                    break;
                default:
                    throw new NotImplementedException("Unknown Animation Style, cannot draw!");
            }
            Game1.spriteBatch.Draw(
                texture: this.Texture,
                position: pos,
                sourceRectangle: rect,
                color: color);
        }
    }
}

