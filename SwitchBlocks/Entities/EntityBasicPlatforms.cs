using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering basic platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityBasicPlatforms : EntityPlatforms
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

        public void Reset()
        {
            DataBasic.Progress = progress;
            instance = null;
        }

        private EntityBasicPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.BASIC);
            progress = DataBasic.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataBasic.State, deltaTime, SettingsBasic.Multiplier);
        }

        public override void Draw()
        {
            if (!UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            SpriteBatch spriteBatch = Game1.spriteBatch;
            foreach (Platform platform in currentPlatformList)
            {
                DrawPlatform(platform, progress, DataBasic.State, spriteBatch);
            }
        }
    }
}
