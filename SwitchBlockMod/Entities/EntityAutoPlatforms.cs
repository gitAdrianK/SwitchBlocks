using EntityComponent;
using JumpKing;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;
using System.Collections.Generic;

namespace SwitchBlocksMod.Entities
{
    public class EntityAutoPlatforms : Entity
    {
        private static EntityAutoPlatforms instance;
        public static EntityAutoPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityAutoPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            instance = null;
        }

        private EntityAutoPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("auto");
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

            if (multiplier != 1.0f && DataAuto.State)
            {
                multiplier += 5.0f * deltaTime;
                if (multiplier >= 1.0f)
                {
                    multiplier = 1.0f;
                }
            }
            else if (multiplier != 0.0f && !DataAuto.State)
            {
                multiplier -= 5.0f * deltaTime;
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

            DataAuto.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();
        }
        private void ThirdElapsed()
        {
            if (DataAuto.RemainingTime <= ModBlocks.AUTO_DURATION * 0.66 && !DataAuto.HasBlinkedOnce)
            {
                if (ModSounds.AUTO_BLINK != null)
                {
                    ModSounds.AUTO_BLINK.Play();
                }
                DataAuto.HasBlinkedOnce = true;
                return;
            }
            if (DataAuto.RemainingTime <= ModBlocks.AUTO_DURATION * 0.33 && !DataAuto.HasBlinkedTwice)
            {
                if (ModSounds.AUTO_BLINK != null)
                {
                    ModSounds.AUTO_BLINK.Play();
                }
                DataAuto.HasBlinkedTwice = true;
                return;
            }
            if (DataAuto.RemainingTime <= 0.0f)
            {
                if (ModSounds.AUTO_FLIP != null)
                {
                    ModSounds.AUTO_FLIP.Play();
                }
                DataAuto.State = !DataAuto.State;
                DataAuto.RemainingTime = ModBlocks.AUTO_DURATION; ;
                DataAuto.HasBlinkedOnce = false;
                DataAuto.HasBlinkedTwice = false;
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
