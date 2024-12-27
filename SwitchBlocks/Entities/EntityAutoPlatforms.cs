namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using JumpKing;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Platforms;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Entity responsible for rendering auto platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityAutoPlatforms : EntityPlatforms
    {
        private static EntityAutoPlatforms instance;
        public static EntityAutoPlatforms Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new EntityAutoPlatforms();
                }
                return instance;
            }
        }

        public void Reset()
        {
            DataAuto.Progress = this.Progress;
            instance = null;
        }

        private EntityAutoPlatforms()
        {
            this.PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.AUTO);
            this.Progress = DataAuto.Progress;
        }

        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(DataAuto.State, deltaTime, SettingsAuto.Multiplier);

            var currentTick = AchievementManager.GetTicks();
            var adjustedTick = (currentTick + SettingsAuto.DurationCycle - DataAuto.ResetTick) % SettingsAuto.DurationCycle;
            this.TrySound(adjustedTick);
            this.TrySwitch(adjustedTick);
        }

        public override void Draw()
        {
            if (!this.UpdateCurrentScreen() || EndingManager.HasFinished)
            {
                return;
            }

            var spriteBatch = Game1.spriteBatch;
            _ = Parallel.ForEach(this.CurrentPlatformList, platform
                => DrawPlatform(platform, this.Progress, DataAuto.State, spriteBatch));
        }

        private void TrySound(int adjustedTick)
        {
            if (DataAuto.State)
            {
                adjustedTick += SettingsAuto.DurationOff;
            }
            var soundAdjust = (SettingsAuto.WarnCount - DataAuto.WarnCount) * SettingsAuto.WarnDuration;
            var soundTick = (adjustedTick + soundAdjust) % SettingsAuto.DurationCycle;
            // Its not yet time to make a sound
            if (soundTick != 0)
            {
                return;
            }
            // Check which sound is to be played
            if (SettingsAuto.WarnCount == DataAuto.WarnCount)
            {
                this.DoFlipSound();
            }
            else
            {
                this.DoWarnSound();
            }
        }

        private void DoWarnSound()
        {
            DataAuto.WarnCount++;
            if (!this.IsActiveOnCurrentScreen)
            {
                return;
            }
            // The sound was disabled
            if (DataAuto.State)
            {
                if (SettingsAuto.WarnDisableOn)
                {
                    return;
                }
            }
            else
            {
                if (SettingsAuto.WarnDisableOff)
                {
                    return;
                }
            }
            ModSounds.AutoWarn?.PlayOneShot();
        }

        private void DoFlipSound()
        {
            DataAuto.WarnCount = 0;
            if (!this.IsActiveOnCurrentScreen)
            {
                return;
            }
            ModSounds.AutoFlip?.PlayOneShot();
        }

        private void TrySwitch(int adjustedTick)
        {
            // I think its < but it could be <=
            var currState = adjustedTick - SettingsAuto.DurationOn < 0;
            if (DataAuto.State == currState)
            {
                return;
            }

            if (DataAuto.CanSwitchSafely && DataAuto.SwitchOnceSafe)
            {
                DataAuto.State = currState;
                DataAuto.SwitchOnceSafe = false;
                return;
            }


            if (DataAuto.CanSwitchSafely || SettingsAuto.ForceSwitch)
            {
                DataAuto.State = currState;
            }
            else
            {
                DataAuto.SwitchOnceSafe = true;
            }
        }
    }
}
