using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using System;

namespace SwitchBlocksMod.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySandPlatforms : EntityPlatforms
    {
        private static EntitySandPlatforms instance;
        public static EntitySandPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntitySandPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataCountdown.Progress = progress;
            instance = null;
        }

        private EntitySandPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.SAND);
        }

        float offset;

        protected override void Update(float deltaTime)
        {
            offset += deltaTime;
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
            int scroll = Math.Abs((int)offset % (platform.texture.Height - platform.size.Y + 1));
            if (platform.startState == DataSand.State)
            {
                // Scrolling inside
                spriteBatch.Draw(
                texture: platform.texture,
                position: platform.position,
                sourceRectangle: new Rectangle(
                    platform.size.X,
                    scroll,
                    platform.size.X,
                    platform.size.Y),
                color: Color.White);
                // Overlay outside
                spriteBatch.Draw(
                texture: platform.texture,
                position: platform.position,
                sourceRectangle: new Rectangle(
                    0,
                    0,
                    platform.size.X,
                    platform.size.Y),
                color: Color.White);
                return;
            }
            // Scrolling inside
            spriteBatch.Draw(
            texture: platform.texture,
            position: platform.position,
            sourceRectangle: new Rectangle(
                platform.size.X,
                platform.texture.Height - platform.size.Y - scroll,
                platform.size.X,
                platform.size.Y),
            color: Color.White);
            // Overlay outside
            spriteBatch.Draw(
            texture: platform.texture,
            position: platform.position,
            sourceRectangle: new Rectangle(
                0,
                platform.size.Y,
                platform.size.X,
                platform.size.Y),
            color: Color.White);
        }
    }
}
