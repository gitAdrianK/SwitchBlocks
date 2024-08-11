using SwitchBlocks.Data;
using SwitchBlocks.Util;

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
            UpdateProgress(DataCountdown.State, deltaTime, ModBlocks.CountdownMultiplier);

            if (!DataCountdown.State)
            {
                return;
            }
            DataCountdown.RemainingTime -= deltaTime * 0.5f;
            TryWarn();
            TrySwitch();
        }

        private void TryWarn()
        {
            if (ModSounds.CountdownWarn == null || DataCountdown.WarnCount == ModBlocks.CountdownWarnCount)
            {
                return;
            }
            if (DataCountdown.RemainingTime <= ModBlocks.CountdownDuration - ModBlocks.CountdownDuration * ((DataCountdown.WarnCount + 1) / (ModBlocks.CountdownWarnCount + 1)))
            {
                ModSounds.CountdownWarn.PlayOneShot();
                DataCountdown.WarnCount++;
            }
        }

        private void TrySwitch()
        {
            if (DataCountdown.CanSwitchSafely && DataCountdown.SwitchOnceSafe)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }
                DataCountdown.State = false;
                DataCountdown.RemainingTime = ModBlocks.CountdownDuration;
                DataCountdown.SwitchOnceSafe = false;
                DataCountdown.WarnCount = 0;
                return;
            }
            if (DataCountdown.RemainingTime <= 0.0f)
            {
                if (DataCountdown.CanSwitchSafely || ModBlocks.CountdownForceSwitch)
                {
                    if (currentPlatformList != null)
                    {
                        ModSounds.CountdownFlip?.PlayOneShot();
                    }
                    DataCountdown.State = false;
                }
                else
                {
                    DataCountdown.SwitchOnceSafe = true;
                }
                DataCountdown.RemainingTime = ModBlocks.CountdownDuration;
            }
        }
    }
}
