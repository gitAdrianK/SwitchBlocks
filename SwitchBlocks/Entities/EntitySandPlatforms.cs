using EntityComponent;
using JumpKing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Util;
using System.Collections.Generic;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering sand platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntitySandPlatforms : Entity
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
            instance = null;
        }

        private EntitySandPlatforms()
        {
            PlatformDictionary = PlatformSand.GetPlatformsDictonary(ModStrings.SAND);
        }

        private float offset;

        private int currentScreen = -1;
        private int nextScreen;

        public Dictionary<int, List<PlatformSand>> PlatformDictionary { get; protected set; }
        private List<PlatformSand> currentPlatformList;

        protected override void Update(float deltaTime)
        {
            offset += deltaTime * ModBlocks.sandMultiplier;
        }

        public override void Draw()
        {
            if (!UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            foreach (PlatformSand platform in currentPlatformList)
            {
                DrawPlatform(platform, spriteBatch);
            }
        }

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

        private void DrawPlatform(PlatformSand platform, SpriteBatch spriteBatch)
        {
            if (platform.Texture != null)
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
                texture: platform.Texture,
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
