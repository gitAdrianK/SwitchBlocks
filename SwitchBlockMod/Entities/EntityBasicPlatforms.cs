using EntityComponent;
using JumpKing;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using System;
using System.Collections.Generic;

namespace SwitchBlocksMod.Entities
{
    public class EntityBasicPlatforms : Entity, IDisposable
    {
        private static EntityBasicPlatforms instance;
        public static EntityBasicPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityBasicPlatforms();
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

        private EntityBasicPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("basic");
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

            if (multiplier != 1.0f && DataBasic.State)
            {
                multiplier += deltaTime * 5.0f;
                if (multiplier >= 1.0f)
                {
                    multiplier = 1.0f;
                }
            }
            else if (multiplier != 0.0f && !DataBasic.State)
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
