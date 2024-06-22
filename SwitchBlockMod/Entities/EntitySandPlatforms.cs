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
            if (!UpdateCurrentScreen() || ModEntry.HasFinished)
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
            Rectangle sourceRectangle;
            if (platform.StartState == DataSand.State)
            {
                sourceRectangle = new Rectangle(
                    0,
                    0,
                    platform.Width,
                    platform.Height);
            }
            else
            {
                sourceRectangle = new Rectangle(
                    platform.Width,
                    0,
                    platform.Width,
                    platform.Height);
            }

            spriteBatch.Draw(
                texture: platform.Background,
                position: platform.Position,
                sourceRectangle: sourceRectangle,
                color: Color.White);
        }

        private void DrawScrolling(PlatformSand platform, SpriteBatch spriteBatch)
        {
            int actualOffset = (int)(offset % platform.Scrolling.Height);
            actualOffset = platform.StartState == DataSand.State ? actualOffset : platform.Scrolling.Height - actualOffset;

            // Depending on if the offset would make it so we go past the texture.
            if (actualOffset + platform.Height > platform.Scrolling.Height)
            {
                int diff = platform.Scrolling.Height - actualOffset;
                spriteBatch.Draw(
                texture: platform.Scrolling,
                position: platform.Position,
                sourceRectangle: new Rectangle(
                    0,
                    actualOffset,
                    platform.Width,
                    diff),
                color: Color.White);

                spriteBatch.Draw(
                texture: platform.Scrolling,
                position: new Vector2(
                    platform.Position.X,
                    platform.Position.Y + diff),
                sourceRectangle: new Rectangle(
                    0,
                    0,
                    platform.Width,
                    platform.Height - diff),
                color: Color.White);
                return;
            }
            spriteBatch.Draw(
                texture: platform.Scrolling,
                position: platform.Position,
                sourceRectangle: new Rectangle(
                    0,
                    actualOffset,
                    platform.Width,
                    platform.Height),
                color: Color.White);
        }

        private void DrawForeground(PlatformSand platform, SpriteBatch spriteBatch)
        {
            Rectangle sourceRectangle;
            if (platform.StartState == DataSand.State)
            {
                sourceRectangle = new Rectangle(
                    0,
                    0,
                    platform.Width,
                    platform.Height);
            }
            else
            {
                sourceRectangle = new Rectangle(
                    platform.Width,
                    0,
                    platform.Width,
                    platform.Height);
            }
            spriteBatch.Draw(
                texture: platform.Foreground,
                position: platform.Position,
                sourceRectangle: sourceRectangle,
                color: Color.White);
        }
    }
}
