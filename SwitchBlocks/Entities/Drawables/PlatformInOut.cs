using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Util;
using System;
using Curve = SwitchBlocks.Util.Curve;

namespace SwitchBlocks.Entities.Drawables
{
    public class PlatformInOut : Platform
    {
        private const double HALF_PI = Math.PI / 2.0d;

        public Animation Animation { get; set; }
        public Animation AnimationOut { get; set; }

        public override void Draw(SpriteBatch spriteBatch, bool state, float progress)
        {
            float progressAdjusted = StartState ? 1.0f - progress : progress;
            if (progressAdjusted == 0.0f)
            {
                return;
            }
            if (progressAdjusted == 1.0f)
            {
                spriteBatch.Draw(
                    texture: Texture,
                    position: Position,
                    color: Color.White);
                return;
            }

            Animation animation = StartState == state ? Animation : AnimationOut;
            float progressActual;
            switch (animation.Curve)
            {
                case Curve.Linear:
                    progressActual = progressAdjusted;
                    break;
                case Curve.EaseIn:
                    progressActual = (float)Math.Sin(progressAdjusted * HALF_PI - HALF_PI) + 1.0f;
                    break;
                case Curve.EaseOut:
                    progressActual = (float)Math.Sin(progressAdjusted * HALF_PI);
                    break;
                case Curve.EaseInOut:
                    progressActual = (float)(Math.Sin(progressAdjusted * Math.PI - HALF_PI) + 1.0f) / 2.0f;
                    break;
                default:
                    throw new InvalidOperationException("Animation curve was none");
            }

            int height = Texture.Height;
            int width = Texture.Width;
            switch (animation.Style)
            {
                case Style.Fade:
                    spriteBatch.Draw(
                        texture: Texture,
                        position: Position,
                        color: new Color(
                            progressActual,
                            progressActual,
                            progressActual,
                            progressActual));
                    break;
                case Style.Top:
                    int heightTop = (int)(height * progressActual);

                    Rectangle rectangleTop = new Rectangle(
                        0,
                        height - heightTop,
                        width,
                        heightTop);

                    spriteBatch.Draw(
                        texture: Texture,
                        position: Position,
                        sourceRectangle: rectangleTop,
                        color: Color.White);
                    break;
                case Style.Bottom:
                    int heightBottom = (int)(height * progressActual);

                    Vector2 vectorBottom = new Vector2(
                        Position.X,
                        Position.Y + height - heightBottom);

                    Rectangle rectangleBottom = new Rectangle(
                        0,
                        0,
                        width,
                        heightBottom);

                    spriteBatch.Draw(
                        texture: Texture,
                        position: vectorBottom,
                        sourceRectangle: rectangleBottom,
                        color: Color.White);
                    break;
                case Style.Left:
                    int widthLeft = (int)(width * progressActual);

                    Rectangle rectangleLeft = new Rectangle(
                        width - widthLeft,
                        0,
                        widthLeft,
                        height);

                    spriteBatch.Draw(
                        texture: Texture,
                        position: Position,
                        sourceRectangle: rectangleLeft,
                        color: Color.White);
                    break;
                case Style.Right:
                    int widthRight = (int)(width * progressActual);

                    Vector2 vectorRight = new Vector2(
                        Position.X + width - widthRight,
                        Position.Y);

                    Rectangle rectangleRight = new Rectangle(
                        0,
                        0,
                        widthRight,
                        height);

                    spriteBatch.Draw(
                        texture: Texture,
                        position: vectorRight,
                        sourceRectangle: rectangleRight,
                        color: Color.White);
                    break;
                default:
                    throw new InvalidOperationException("Animation style was none");
            }
        }

        public override bool InitializeOthers()
        {
            Style animStyle = Animation.Style != Style.None ? Animation.Style : Style.Fade;
            Curve animCurve = Animation.Curve != Curve.None ? Animation.Curve : Curve.Linear;

            Style animOutStyle = AnimationOut.Style != Style.None ? AnimationOut.Style : animStyle;
            Curve animOutCurve = AnimationOut.Curve != Curve.None ? AnimationOut.Curve : animCurve;

            Animation = new Animation { Style = animStyle, Curve = animCurve };
            AnimationOut = new Animation { Style = animOutStyle, Curve = animOutCurve };

            return true;
        }
    }
}
