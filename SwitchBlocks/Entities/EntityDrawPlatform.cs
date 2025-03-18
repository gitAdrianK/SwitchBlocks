namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util;
    using SwitchBlocks.Util.Deserialization;
    using static SwitchBlocks.Util.Animation;
    using Curve = Util.Animation.Curve;

    public class EntityDrawPlatform : EntityDraw
    {
        protected bool StartState { get; }
        protected Animation Animation { get; }
        protected Animation AnimationOut { get; }
        protected IDataProvider Data { get; }

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

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || EndingManager.HasFinished)
            {
                return;
            }
            this.DrawWithRectangle(new Rectangle(0, 0, this.Width, this.Height));
        }

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
            switch (animation.AnimCurve)
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
            switch (animation.AnimStyle)
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

