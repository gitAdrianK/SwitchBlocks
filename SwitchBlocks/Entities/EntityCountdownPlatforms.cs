using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;
using SwitchBlocks.Settings;

namespace SwitchBlocks.Entities
{
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
            DataCountdown.Progress = progress;
            instance = null;
        }

        private EntityCountdownPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.COUNTDOWN);
            progress = DataCountdown.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataCountdown.State, deltaTime, SettingsCountdown.Multiplier);

            if (!DataCountdown.State)
            {
                return;
            }

            int currentTick = AchievementManager.GetTicks();
            int adjustedTick = SettingsCountdown.Duration - (currentTick - DataCountdown.ActivatedTick);
            if (IsActiveOnCurrentScreen)
            {
                TryWarn(adjustedTick);
            }
            TrySwitch(currentTick);
        }

        private void TryWarn(int adjustedTick)
        {
            if (ModSounds.CountdownWarn == null || DataCountdown.WarnCount == SettingsCountdown.WarnCount)
            {
                return;
            }
            int warnAdjust = (SettingsCountdown.WarnCount - DataCountdown.WarnCount) * SettingsCountdown.WarnDuration;
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
                if (IsActiveOnCurrentScreen)
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
                    if (IsActiveOnCurrentScreen)
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
