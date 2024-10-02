using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static SwitchBlocks.Util.Animation;
using Curve = SwitchBlocks.Util.Animation.Curve;

namespace SwitchBlocks.Entities
{
    public class EntityPlatforms : Entity
    {
        int currentScreen = -1;
        int nextScreen;

        protected float progress;
        public Dictionary<int, List<Platform>> PlatformDictionary { get; protected set; }
        protected List<Platform> currentPlatformList;

        public bool IsActiveOnCurrentScreen => currentPlatformList != null;

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

            nextScreen = Camera.CurrentScreen;
            if (currentScreen != nextScreen)
            {
                currentPlatformList = null;
                if (PlatformDictionary.ContainsKey(nextScreen))
                {
                    currentPlatformList = PlatformDictionary[nextScreen];
                }
                currentScreen = nextScreen;
            }

            if (currentPlatformList == null)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Updates the progress of the platform that is used when animating.
        /// </summary>
        /// <param name="state">State of the platforms type</param>
        /// <param name="amount">Amount to be added/subtracted from the progress</param>
        /// <param name="multiplier">Multiplier of the amount added/subtracted</param>
        protected void UpdateProgress(bool state, float amount, float multiplier)
        {
            // This multiplication by two is to keep parity with a previous bug that would see the value doubled.
            amount *= 2.0f;
            amount *= multiplier;
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
            if (!UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            Parallel.ForEach(currentPlatformList, platform =>
            {
                DrawPlatform(platform, spriteBatch);
            });
        }

        public static void DrawPlatform(Platform platform, float progress, SpriteBatch spriteBatch)
        {
            if (progress == 0.0f)
            {
                return;
            }
            if (progress == 1.0f)
            {
                spriteBatch.Draw(
                    texture: platform.Texture,
                    position: platform.Position,
                    color: Color.White);
                return;
            }

            float progressActual = 1.0f;
            switch (platform.Animation.curve)
            {
                case Curve.Linear:
                    progressActual = progress;
                    break;
                case Curve.EaseIn:
                    progressActual = (float)Math.Sin(progress * HALF_PI - HALF_PI) + 1.0f;
                    break;
                case Curve.EaseOut:
                    progressActual = (float)Math.Sin(progress * HALF_PI);
                    break;
                case Curve.EaseInOut:
                    progressActual = (float)(Math.Sin(progress * Math.PI - HALF_PI) + 1.0f) / 2.0f;
                    break;
            }

            int height = platform.Height;
            int width = platform.Width;
            switch (platform.Animation.style)
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
                    int heightTop = (int)(height * progressActual);
                    Rectangle rectangleTop = new Rectangle(
                        0,
                        height - heightTop,
                        width,
                        heightTop);

                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: platform.Position,
                        sourceRectangle: rectangleTop,
                        color: Color.White);
                    break;

                case Style.Bottom:
                    int heightBottom = (int)(height * progressActual);

                    Vector2 vectorBottom = new Vector2(
                        platform.Position.X,
                        platform.Position.Y + height - heightBottom);

                    Rectangle rectangleBottom = new Rectangle(
                        0,
                        0,
                        width,
                        heightBottom);

                    spriteBatch.Draw(
                        texture: platform.Texture,
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
                        texture: platform.Texture,
                        position: platform.Position,
                        sourceRectangle: rectangleLeft,
                        color: Color.White);
                    break;

                case Style.Right:
                    int widthRight = (int)(width * progressActual);

                    Vector2 vectorRight = new Vector2(
                        platform.Position.X + width - widthRight,
                        platform.Position.Y);

                    Rectangle rectangleRight = new Rectangle(
                        0,
                        0,
                        widthRight,
                        height);

                    spriteBatch.Draw(
                        texture: platform.Texture,
                        position: vectorRight,
                        sourceRectangle: rectangleRight,
                        color: Color.White);
                    break;
            }
        }

        private void DrawPlatform(Platform platform, SpriteBatch spriteBatch)
        {
            float progressAdjusted = platform.StartState ? 1.0f - progress : progress;
            DrawPlatform(platform, progressAdjusted, spriteBatch);
        }
    }
}
