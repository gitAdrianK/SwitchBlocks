using EntityComponent;
using JumpKing;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using static SwitchBlocksMod.Util.Animation;
using Curve = SwitchBlocksMod.Util.Animation.Curve;

namespace SwitchBlocksMod.Entities
{
    public class EntityPlatforms : Entity
    {
        int currentScreen = -1;
        int nextScreen;

        protected float progress;
        public Dictionary<int, List<Platform>> PlatformDictionary { get; protected set; }
        List<Platform> currentPlatformList;

        protected bool UpdateCurrentScreen()
        {
            if (PlatformDictionary == null)
            {
                return false;
            }

            nextScreen = LevelManager.CurrentScreen.GetIndex0();
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

        public override void Draw()
        {
            if (currentPlatformList == null)
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
            float progressAdjusted = platform.startState ? progress : 1.0f - progress;
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

            float progressActual;
            switch (platform.animation.curve)
            {
                case Curve.Stepped:
                    progressActual = progressAdjusted < 0.5f ? 1.0f : 0.0f;
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
                default:
                    progressActual = progressAdjusted;
                    break;
            }

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
                        progressActual)
                    );
                    break;
                case Style.Top:
                    //TODO: Animation Top
                    break;
                case Style.Bottom:
                    //TODO: Animation Bottom
                    break;
                case Style.Left:
                    //TODO: Animation Left
                    break;
                case Style.Right:
                    //TODO: Animation Right
                    break;
            }
        }
    }
}
