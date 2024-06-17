using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Util;
using System;
using System.Collections.Generic;
using static SwitchBlocksMod.Util.Animation;
using Curve = SwitchBlocksMod.Util.Animation.Curve;

namespace SwitchBlocksMod.Entities
{
    public class EntityPlatforms : Entity
    {

        protected float progress;
        public Dictionary<int, List<Platform>> PlatformDictionary { get; protected set; }
        protected List<Platform> currentPlatformList;

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>false if no platforms are to be drawn, true otherwise</returns>
        protected bool UpdateCurrentScreen()
        {
            if (PlatformDictionary == null)
            {
                return false;
            }
            int currentScreen = Camera.CurrentScreen;
            if (PlatformDictionary.ContainsKey(currentScreen))
            {
                currentPlatformList = PlatformDictionary[currentScreen];
                return true;
            }
            else
            {
                currentPlatformList = null;
                return false;
            }
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="state">State of the platforms type</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(bool state, float amount, float multiplier)
        {
            amount = amount * multiplier;
            if (progress != 1.0f && state)
            {
                progress += amount;
                if (progress >= 1.0f)
                {
                    progress = 1.0f;
                }
            }
            else if (progress != 0.0f && !state)
            {
                progress -= amount;
                if (progress <= 0.0f)
                {
                    progress = 0.0f;
                }
            }
        }

        public override void Draw()
        {
            if (!UpdateCurrentScreen())
            {
                return;
            }


            SpriteBatch spriteBatch = Game1.spriteBatch;
            foreach (Platform platform in currentPlatformList)
            {
                DrawPlatform(platform, spriteBatch);
            }
        }

        private void DrawPlatform(Platform platform, SpriteBatch spriteBatch)
        {
            //CONSIDER: Visual feedback for blinking.
            float progressAdjusted = platform.startState ? 1.0f - progress : progress;
            if (progressAdjusted == 0.0f)
            {
                return;
            }
            if (progressAdjusted == 1.0f)
            {
                spriteBatch.Draw(
                    texture: platform.texture,
                    position: platform.position,
                    color: Color.White);
                return;
            }

            float progressActual = 1.0f;
            switch (platform.animation.curve)
            {
                case Curve.Stepped:
                    progressActual = progressAdjusted < 0.9f ? 1.0f : 0.0f;
                    break;
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
            }

            int height = platform.texture.Height;
            int width = platform.texture.Width;
            switch (platform.animation.style)
            {
                case Style.Fade:
                    spriteBatch.Draw(
                        texture: platform.texture,
                        position: platform.position,
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
                        texture: platform.texture,
                        position: platform.position,
                        sourceRectangle: rectangleTop,
                        color: Color.White);
                    break;

                case Style.Bottom:
                    int heightBottom = (int)(height * progressActual);

                    Vector2 vectorBottom = new Vector2(
                        platform.position.X,
                        platform.position.Y + height - heightBottom);

                    Rectangle rectangleBottom = new Rectangle(
                        0,
                        0,
                        width,
                        heightBottom);

                    spriteBatch.Draw(
                        texture: platform.texture,
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
                        texture: platform.texture,
                        position: platform.position,
                        sourceRectangle: rectangleLeft,
                        color: Color.White);
                    break;

                case Style.Right:
                    int widthRight = (int)(width * progressActual);

                    Vector2 vectorRight = new Vector2(
                        platform.position.X + width - widthRight,
                        platform.position.Y);

                    Rectangle rectangleRight = new Rectangle(
                        0,
                        0,
                        widthRight,
                        height);

                    spriteBatch.Draw(
                        texture: platform.texture,
                        position: vectorRight,
                        sourceRectangle: rectangleRight,
                        color: Color.White);
                    break;
            }
        }
    }
}
