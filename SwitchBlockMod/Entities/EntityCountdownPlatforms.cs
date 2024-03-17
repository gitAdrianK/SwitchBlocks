using EntityComponent;
using JumpKing;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;
using System;
using System.Collections.Generic;

namespace SwitchBlocksMod.Entities
{
    public class EntityCountdownPlatforms : Entity, IDisposable
    {
        private static EntityCountdownPlatforms instance;
        public static EntityCountdownPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityCountdownPlatforms();
                }
                return instance;
            }
        }

        public void Dispose()
        {
            foreach (List<Platform> list in PlatformDictionary.Values)
            {
                foreach (Platform platform in list)
                {
                    platform.texture.Dispose();
                }
            }
            PlatformDictionary = null;
            currentPlatformList = null;
            instance = null;
        }

        private EntityCountdownPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("countdown");
        }

        int currentScreen = -1;
        int nextScreen;

        float multiplier;
        Color colorOn = Color.White;
        Color colorOff = Color.Black;
        Color colorNothing = Color.Transparent;

        public Dictionary<int, List<Platform>> PlatformDictionary { get; private set; }
        List<Platform> currentPlatformList;

        protected override void Update(float deltaTime)
        {
            if (PlatformDictionary == null)
            {
                return;
            }

            nextScreen = LevelManager.CurrentScreen.GetIndex0();
            if (currentScreen != nextScreen)
            {
                if (PlatformDictionary.ContainsKey(nextScreen))
                {
                    currentPlatformList = PlatformDictionary[nextScreen];
                }
                else
                {
                    currentPlatformList = null;
                }
                currentScreen = nextScreen;
            }

            if (currentPlatformList == null)
            {
                return;
            }

            if (multiplier != 1.0f && DataCountdown.State)
            {
                multiplier += deltaTime * 5.0f;
                if (multiplier >= 1.0f)
                {
                    multiplier = 1.0f;
                }
            }
            else if (multiplier != 0.0f && !DataCountdown.State)
            {
                multiplier -= deltaTime * 5.0f;
                if (multiplier <= 0.0f)
                {
                    multiplier = 0.0f;
                }
            }

            byte nextColorOn = (byte)(255 * (1.0f - multiplier));
            byte nextColorOff = (byte)(255 - nextColorOn);
            colorOn.R = nextColorOn;
            colorOn.G = nextColorOn;
            colorOn.B = nextColorOn;
            colorOn.A = nextColorOn;
            colorOff.R = nextColorOff;
            colorOff.G = nextColorOff;
            colorOff.B = nextColorOff;
            colorOff.A = nextColorOff;

            if (!DataCountdown.State)
            {
                return;
            }
            DataCountdown.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();
        }

        private void ThirdElapsed()
        {
            if (currentPlatformList == null)
            {
                return;
            }
            if (DataCountdown.RemainingTime <= ModBlocks.COUNTDOWN_DURATION * 0.66 && !DataCountdown.HasBlinkedOnce)
            {
                if (ModSounds.COUNTDOWN_BLINK != null)
                {
                    ModSounds.COUNTDOWN_BLINK.Play();
                }
                DataCountdown.HasBlinkedOnce = true;
                return;
            }
            if (DataCountdown.RemainingTime <= ModBlocks.COUNTDOWN_DURATION * 0.33 && !DataCountdown.HasBlinkedTwice)
            {
                if (ModSounds.COUNTDOWN_BLINK != null)
                {
                    ModSounds.COUNTDOWN_BLINK.Play();
                }
                DataCountdown.HasBlinkedTwice = true;
                return;
            }
            if (DataCountdown.RemainingTime <= 0.0f)
            {
                if (ModSounds.COUNTDOWN_FLIP != null)
                {
                    ModSounds.COUNTDOWN_FLIP.Play();
                }
                DataCountdown.State = false;
                DataCountdown.RemainingTime = ModBlocks.COUNTDOWN_DURATION; ;
                DataCountdown.HasBlinkedOnce = false;
                DataCountdown.HasBlinkedTwice = false;
            }
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
            Color color = platform.startState ? colorOn : colorOff;
            if (color == colorNothing)
            {
                return;
            }
            spriteBatch.Draw(
                texture: platform.texture,
                position: platform.position,
                color: color);
        }
    }
}
