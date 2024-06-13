using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;
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
            PlatformDictionary = PlatformSand.GetPlatformsDictonary(ModStrings.SAND);
        }

        float offset;

        protected override void Update(float deltaTime)
        {
            offset += deltaTime * ModBlocks.sandMultiplier;
        }

        public override void Draw()
        {
            if (!UpdateCurrentScreen())
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            foreach (PlatformSand platform in currentPlatformList)
            {
                DrawPlatform(platform, spriteBatch);
            }
        }

        private void DrawPlatform(PlatformSand platform, SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangleBackground;
            if (platform.startState == DataSand.State)
            {
                sourceRectangleBackground = new Rectangle(
                    0,
                    0,
                    platform.background.Width / 2,
                    platform.background.Height);
            }
            else
            {
                sourceRectangleBackground = new Rectangle(
                    platform.background.Width / 2,
                    0,
                    platform.background.Width / 2,
                    platform.background.Height);
            }

            // Background
            spriteBatch.Draw(
                texture: platform.background,
                position: platform.position,
                sourceRectangle: sourceRectangleBackground,
                color: Color.White);

            // Scrolling
            int heightDifference = platform.scrolling.Height - platform.background.Height;
            if (heightDifference > 0)
            {
                int scroll = Math.Abs((int)offset % (heightDifference + 1));

                Rectangle sourceRectangleScrolling;
                if (platform.startState == DataSand.State)
                {
                    sourceRectangleScrolling = new Rectangle(
                        0,
                        scroll,
                        platform.scrolling.Width,
                        platform.background.Height);
                }
                else
                {
                    sourceRectangleScrolling = new Rectangle(
                        0,
                        heightDifference - scroll,
                        platform.scrolling.Width,
                        platform.background.Height);
                }

                spriteBatch.Draw(
                    texture: platform.scrolling,
                    position: platform.position,
                    sourceRectangle: sourceRectangleScrolling,
                    color: Color.White);
            }

            // Foreground
            if (platform.foreground == null)
            {
                return;
            }

            Rectangle sourceRectangleForeground;
            if (platform.startState == DataSand.State)
            {
                sourceRectangleForeground = new Rectangle(
                    0,
                    0,
                    platform.foreground.Width / 2,
                    platform.foreground.Height);
            }
            else
            {
                sourceRectangleForeground = new Rectangle(
                    platform.foreground.Width / 2,
                    0,
                    platform.foreground.Width / 2,
                    platform.foreground.Height);
            }
            spriteBatch.Draw(
                texture: platform.foreground,
                position: platform.position,
                sourceRectangle: sourceRectangleForeground,
                color: Color.White);
        }
    }
}
