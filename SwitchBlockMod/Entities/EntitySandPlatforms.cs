using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;

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
            if (platform.Background != null)
            {
                DrawBackground(platform, spriteBatch);
            }

            if (platform.Scrolling != null)
            {
                DrawScrolling(platform, spriteBatch);
            }

            if (platform.Foreground != null)
            {
                DrawForeground(platform, spriteBatch);
            }
        }

        private void DrawBackground(PlatformSand platform, SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangleBackground;
            if (platform.StartState == DataSand.State)
            {
                sourceRectangleBackground = new Rectangle(
                    0,
                    0,
                    platform.Width,
                    platform.Height);
            }
            else
            {
                sourceRectangleBackground = new Rectangle(
                    platform.Width,
                    0,
                    platform.Width,
                    platform.Height);
            }

            spriteBatch.Draw(
                texture: platform.Background,
                position: platform.Position,
                sourceRectangle: sourceRectangleBackground,
                color: Color.White);
        }

        private void DrawScrolling(PlatformSand platform, SpriteBatch spriteBatch)
        {
            // TODO: Rework so any texture at least the size of the platform width/height works.
        }

        private void DrawForeground(PlatformSand platform, SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangleForeground;
            if (platform.StartState == DataSand.State)
            {
                sourceRectangleForeground = new Rectangle(
                    0,
                    0,
                    platform.Width,
                    platform.Height);
            }
            else
            {
                sourceRectangleForeground = new Rectangle(
                    platform.Width,
                    0,
                    platform.Width,
                    platform.Height);
            }
            spriteBatch.Draw(
                texture: platform.Foreground,
                position: platform.Position,
                sourceRectangle: sourceRectangleForeground,
                color: Color.White);
        }
    }
}
