namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using EntityComponent;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Platforms;
    using static SwitchBlocks.Util.Animation;
    using Curve = Util.Animation.Curve;

    public class EntityPlatforms : Entity
    {
        private int currentScreen = -1;
        private int nextScreen;

        protected float Progress { get; set; }
        public Dictionary<int, List<Platform>> PlatformDictionary { get; protected set; }
        protected List<Platform> CurrentPlatformList { get; set; }

        public bool IsActiveOnCurrentScreen => this.CurrentPlatformList != null;

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>false if no platforms are to be drawn, true otherwise</returns>
        protected bool UpdateCurrentScreen()
        {
            if (this.PlatformDictionary == null)
            {
                return false;
            }

            this.nextScreen = Camera.CurrentScreen;
            if (this.currentScreen != this.nextScreen)
            {
                _ = this.PlatformDictionary.TryGetValue(this.nextScreen, out var value);
                this.CurrentPlatformList = value;
                this.currentScreen = this.nextScreen;
            }
            return this.CurrentPlatformList != null;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="state">State of the platforms type</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(bool state, float amount, float multiplier)
        {
            var stateInt = Convert.ToInt32(state);
            if (this.Progress == stateInt)
            {
                return;
            }
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= (-1 + (stateInt * 2)) * 2 * multiplier;
            this.Progress += amount;
            this.Progress = Math.Min(Math.Max(this.Progress, 0), 1);
        }

        public static void DrawPlatform(Platform platform, float progress, bool state, SpriteBatch spriteBatch)
        {
            var progressAdjusted = platform.StartState ? 1.0f - progress : progress;
            if (progressAdjusted == 0.0f)
            {
                return;
            }
            if (progressAdjusted == 1.0f)
            {
                spriteBatch.Draw(
                    texture: platform.Texture,
                    position: platform.Position,
                    color: Color.White);
                return;
            }

            float progressActual;
            var animation = platform.StartState == state ? platform.Animation : platform.AnimationOut;
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

            var height = platform.Height;
            var width = platform.Width;
            switch (animation.AnimStyle)
            {
                case Style.Fade:
                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: platform.Position,
                        color: new Color(
                            progressActual,
                            progressActual,
                            progressActual,
                            progressActual));
                    break;

                case Style.Top:
                    var heightTop = (int)(height * progressActual);

                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: platform.Position,
                        sourceRectangle: new Rectangle(
                            0,
                            height - heightTop,
                            width,
                            heightTop),
                        color: Color.White);
                    break;

                case Style.Bottom:
                    var heightBottom = (int)(height * progressActual);

                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: new Vector2(
                            platform.Position.X,
                            platform.Position.Y + height - heightBottom),
                        sourceRectangle: new Rectangle(
                            0,
                            0,
                            width,
                            heightBottom),
                        color: Color.White);
                    break;

                case Style.Left:
                    var widthLeft = (int)(width * progressActual);

                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: platform.Position,
                        sourceRectangle: new Rectangle(
                            width - widthLeft,
                            0,
                            widthLeft,
                            height),
                        color: Color.White);
                    break;

                case Style.Right:
                    var widthRight = (int)(width * progressActual);

                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: new Vector2(
                            platform.Position.X + width - widthRight,
                            platform.Position.Y),
                        sourceRectangle: new Rectangle(
                            0,
                            0,
                            widthRight,
                            height),
                        color: Color.White);
                    break;
                default:
                    throw new NotImplementedException("Unknown Animation Style, cannot draw!");
            }
        }
    }
}
