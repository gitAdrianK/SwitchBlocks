namespace SwitchBlocks.Entities
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using EntityComponent;
    using JumpKing;
    using Microsoft.Xna.Framework;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Platforms;
    using SwitchBlocks.Settings;

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

        public void Reset() => instance = null;

        private EntitySandPlatforms() => this.PlatformDictionary = PlatformSand.GetPlatformsDictonary(ModStrings.SAND);

        private float offset;

        private int currentScreen = -1;
        private int nextScreen;

        public Dictionary<int, List<PlatformSand>> PlatformDictionary { get; protected set; }
        private List<PlatformSand> currentPlatformList;

        protected override void Update(float deltaTime) => this.offset += deltaTime * SettingsSand.Multiplier;

        public override void Draw()
        {
            if (!this.UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.currentPlatformList, platform
                => this.DrawPlatform(platform, spriteBatch));
        }

        /// <summary>
        /// Updates what screen is currently active and gets the platforms from the platform dictionary
        /// </summary>
        /// <returns>false if no platforms are to be drawn, true otherwise</returns>
        protected bool UpdateCurrentScreen()
        {
            if (this.PlatformDictionary == null)
            {
                return false;
            }

            this.nextScreen = Camera.CurrentScreen;
            if (this.currentScreen != this.nextScreen)
            {
                _ = this.PlatformDictionary.TryGetValue(this.nextScreen, out this.currentPlatformList);
                this.currentScreen = this.nextScreen;
            }
            return this.currentPlatformList != null;
        }

        private void DrawPlatform(PlatformSand platform, SpriteBatch spriteBatch)
        {
            if (platform.Texture != null)
            {
                this.DrawBackground(platform, spriteBatch);
            }

            if (platform.Scrolling != null)
            {
                this.DrawScrolling(platform, spriteBatch);
            }

            if (platform.Foreground != null)
            {
                this.DrawForeground(platform, spriteBatch);
            }
        }

        private void DrawBackground(PlatformSand platform, SpriteBatch spriteBatch)
        {
            var sourceRectangle = new Rectangle(
                    platform.Width * Convert.ToInt32(platform.StartState != DataSand.State),
                    0,
                    platform.Width,
                    platform.Height);

            spriteBatch.Draw(
                texture: platform.Texture,
                position: platform.Position,
                sourceRectangle: sourceRectangle,
                color: Color.White);
        }

        private void DrawScrolling(PlatformSand platform, SpriteBatch spriteBatch)
        {
            var actualOffset = (int)(this.offset % platform.Scrolling.Height);
            actualOffset = platform.StartState == DataSand.State ? actualOffset : platform.Scrolling.Height - actualOffset;

            // Depending on if the offset would make it so we go past the texture.
            if (actualOffset + platform.Height > platform.Scrolling.Height)
            {
                var diff = platform.Scrolling.Height - actualOffset;
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
            var sourceRectangle = new Rectangle(
                    platform.Width * Convert.ToInt32(platform.StartState != DataSand.State),
                    0,
                    platform.Width,
                    platform.Height);

            spriteBatch.Draw(
                texture: platform.Foreground,
                position: platform.Position,
                sourceRectangle: sourceRectangle,
                color: Color.White);
        }
    }
}
