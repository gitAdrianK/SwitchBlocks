using JumpKing;
using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Entities
{
    public class EntityJumpPlatforms : EntityPlatforms
    {
        private static EntityJumpPlatforms instance;
        public static EntityJumpPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityJumpPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataJump.Progress = progress;
            instance = null;
        }

        private EntityJumpPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.JUMP);
            progress = DataJump.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataJump.State, deltaTime, SettingsJump.Multiplier);
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
                DrawPlatform(platform, progress, DataJump.State, spriteBatch);
            }
        }
    }
}
