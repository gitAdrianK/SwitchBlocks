namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Platforms;
    using SwitchBlocks.Settings;

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
            DataJump.Progress = this.Progress;
            instance = null;
        }

        private EntityJumpPlatforms()
        {
            this.PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.JUMP);
            this.Progress = DataJump.Progress;
        }

        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(DataJump.State, deltaTime, SettingsJump.Multiplier);
            this.TrySwitch();
        }

        public override void Draw()
        {
            if (!this.UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.CurrentPlatformList, platform
                => DrawPlatform(platform, this.Progress, DataJump.State, spriteBatch));
        }

        private void TrySwitch()
        {
            if (DataJump.CanSwitchSafely && DataJump.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.JumpFlip?.PlayOneShot();
                }
                DataJump.State = !DataJump.State;
                DataJump.SwitchOnceSafe = false;
            }
        }
    }
}
