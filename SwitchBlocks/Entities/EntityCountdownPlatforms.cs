namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Platforms;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Entity responsible for rendering countdown platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityCountdownPlatforms : EntityPlatforms
    {
        private static EntityCountdownPlatforms instance;
        public static EntityCountdownPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityCountdownPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataCountdown.Progress = this.Progress;
            instance = null;
        }

        private EntityCountdownPlatforms()
        {
            this.PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.COUNTDOWN);
            this.Progress = DataCountdown.Progress;
        }

        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(DataCountdown.State, deltaTime, SettingsCountdown.Multiplier);

            if (!DataCountdown.State)
            {
                return;
            }

            var currentTick = AchievementManager.GetTicks();
            var adjustedTick = SettingsCountdown.Duration - (currentTick - DataCountdown.ActivatedTick);
            if (this.IsActiveOnCurrentScreen)
            {
                this.TryWarn(adjustedTick);
            }
            this.TrySwitch(currentTick);
        }

        public override void Draw()
        {
            if (!this.UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.CurrentPlatformList, platform
                => DrawPlatform(platform, this.Progress, DataCountdown.State, spriteBatch));
        }

        private void TryWarn(int adjustedTick)
        {
            if (ModSounds.CountdownWarn == null || DataCountdown.WarnCount == SettingsCountdown.WarnCount)
            {
                return;
            }
            var warnAdjust = (SettingsCountdown.WarnCount - DataCountdown.WarnCount) * SettingsCountdown.WarnDuration;
            if (adjustedTick - warnAdjust == 0)
            {
                DataCountdown.WarnCount++;
                ModSounds.CountdownWarn.PlayOneShot();
            }
        }

        private void TrySwitch(int currentTick)
        {
            if (!DataCountdown.State)
            {
                return;
            }

            if (DataCountdown.CanSwitchSafely && DataCountdown.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }
                DataCountdown.State = false;
                DataCountdown.SwitchOnceSafe = false;
                DataCountdown.WarnCount = 0;
                return;
            }

            if (DataCountdown.ActivatedTick + SettingsCountdown.Duration <= currentTick)
            {
                if (DataCountdown.CanSwitchSafely || SettingsCountdown.ForceSwitch)
                {
                    if (this.IsActiveOnCurrentScreen)
                    {
                        ModSounds.CountdownFlip?.PlayOneShot();
                    }
                    DataCountdown.State = false;
                    DataCountdown.WarnCount = 0;
                }
                else
                {
                    DataCountdown.SwitchOnceSafe = true;
                }
            }
        }
    }
}
