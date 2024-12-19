namespace SwitchBlocks.Entities.Drawables
{
    using System;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Util;
    using Curve = Util.Curve;

    public class PlatformInOut : Platform
    {
        private const double HALF_PI = Math.PI / 2.0d;

        public Animation Animation { get; set; }
        public Animation AnimationOut { get; set; }

        public override void Draw(SpriteBatch spriteBatch, bool state, float progress)
        {
            var progressAdjusted = this.StartState ? 1.0f - progress : progress;
            if (progressAdjusted == 0.0f)
            {
                return;
            }
            if (progressAdjusted == 1.0f)
            {
                spriteBatch.Draw(
                    texture: this.Texture,
                    position: this.Position,
                    color: Color.White);
                return;
            }

            var animation = this.StartState == state ? this.Animation : this.AnimationOut;
            float progressActual;
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
                case Curve.None:
                    throw new InvalidOperationException("Animation curve was none");
                default:
                    throw new InvalidOperationException("Animation curve was unknown");
            }

            var height = this.Texture.Height;
            var width = this.Texture.Width;
            switch (animation.Style)
            {
                case Style.Fade:
                    spriteBatch.Draw(
                        texture: this.Texture,
                        position: this.Position,
                        color: new Color(
                            progressActual,
                            progressActual,
                            progressActual,
                            progressActual));
                    break;
                case Style.Top:
                    var heightTop = (int)(height * progressActual);

                    var rectangleTop = new Rectangle(
                        0,
                        height - heightTop,
                        width,
                        heightTop);

                    spriteBatch.Draw(
                        texture: this.Texture,
                        position: this.Position,
                        sourceRectangle: rectangleTop,
                        color: Color.White);
                    break;
                case Style.Bottom:
                    var heightBottom = (int)(height * progressActual);

                    var vectorBottom = new Vector2(
                        this.Position.X,
                        this.Position.Y + height - heightBottom);

                    var rectangleBottom = new Rectangle(
                        0,
                        0,
                        width,
                        heightBottom);

                    spriteBatch.Draw(
                        texture: this.Texture,
                        position: vectorBottom,
                        sourceRectangle: rectangleBottom,
                        color: Color.White);
                    break;
                case Style.Left:
                    var widthLeft = (int)(width * progressActual);

                    var rectangleLeft = new Rectangle(
                        width - widthLeft,
                        0,
                        widthLeft,
                        height);

                    spriteBatch.Draw(
                        texture: this.Texture,
                        position: this.Position,
                        sourceRectangle: rectangleLeft,
                        color: Color.White);
                    break;
                case Style.Right:
                    var widthRight = (int)(width * progressActual);

                    var vectorRight = new Vector2(
                        this.Position.X + width - widthRight,
                        this.Position.Y);

                    var rectangleRight = new Rectangle(
                        0,
                        0,
                        widthRight,
                        height);

                    spriteBatch.Draw(
                        texture: this.Texture,
                        position: vectorRight,
                        sourceRectangle: rectangleRight,
                        color: Color.White);
                    break;
                case Style.None:
                    throw new InvalidOperationException("Animation style was none");
                default:
                    throw new InvalidOperationException("Animation style was unknown");
            }
        }

        public override bool InitializeOthers()
        {
            var animStyle = this.Animation.Style != Style.None ? this.Animation.Style : Style.Fade;
            var animCurve = this.Animation.Curve != Curve.None ? this.Animation.Curve : Curve.Linear;

            var animOutStyle = this.AnimationOut.Style != Style.None ? this.AnimationOut.Style : animStyle;
            var animOutCurve = this.AnimationOut.Curve != Curve.None ? this.AnimationOut.Curve : animCurve;

            this.Animation = new Animation { Style = animStyle, Curve = animCurve };
            this.AnimationOut = new Animation { Style = animOutStyle, Curve = animOutCurve };

            return true;
        }
    }
}
