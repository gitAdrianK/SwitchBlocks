namespace SwitchBlocks.Entities
{
    using System;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Util;
    using static SwitchBlocks.Util.Animation;
    using Curve = Util.Animation.Curve;

    public class EntityDrawPlatformGroup : EntityDraw
    {
        private bool StartState { get; }
        private Animation Animation { get; }
        private Animation AnimationOut { get; }
        private int Height { get; }
        private int Width { get; }
        private int GroupId { get; }
        private IGroupDataProvider Logic { get; }

        public EntityDrawPlatformGroup(
            Texture2D texture,
            Vector2 position,
            bool startState,
            Animation animation,
            Animation animationOut,
            int screen,
            int groupId,
            IGroupDataProvider logic) : base(texture, position, screen)
        {
            this.StartState = startState;
            this.Animation = animation;
            this.AnimationOut = animationOut;
            this.GroupId = groupId;
            this.Logic = logic;
        }

        public override void Draw()
        {
            if (Camera.CurrentScreen != this.Screen || EndingManager.HasFinished)
            {
                return;
            }

            var progressAdjusted = this.StartState ? 1.0f - this.Logic.GetProgress(this.GroupId) : this.Logic.GetProgress(this.GroupId);
            if (progressAdjusted == 0.0f)
            {
                return;
            }
            if (progressAdjusted == 1.0f)
            {
                Game1.spriteBatch.Draw(
                    texture: this.Texture,
                    position: this.Position,
                    color: Color.White);
                return;
            }

            float progressActual;
            var animation = this.StartState == this.Logic.GetState(this.GroupId) ? this.Animation : this.AnimationOut;
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
                default:
                    throw new NotImplementedException("Unknown Animation Curve, cannot draw!");
            }

            var color = Color.White;
            var rect = new Rectangle(0, 0, this.Width, this.Height);
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
