using SwitchBlocks.Data;
using SwitchBlocks.Patching;
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

            int currentTick = AchievementManager.GetTicks();
            if (IsActiveOnCurrentScreen)
            {
                TryWarn(currentTick);
            }
            TrySwitch(currentTick);
        }

        private void TryWarn(int currentTick)
        {
            if (ModSounds.CountdownWarn == null || DataCountdown.WarnCount == ModBlocks.CountdownWarnCount)
            {
                return;
            }
            /*
            if (DataCountdown.RemainingTime <= (ModBlocks.CountdownWarnCount - DataCountdown.WarnCount) * ModBlocks.CountdownWarnDuration)
            {
                ModSounds.CountdownWarn.PlayOneShot();
                DataCountdown.WarnCount++;
            }
            */
        }


        private void TrySwitch(int currentTick)
        {
            if (!DataCountdown.State)
            {
                return;
            }

            if (DataCountdown.CanSwitchSafely && DataCountdown.SwitchOnceSafe)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }
                DataCountdown.State = false;
                DataCountdown.SwitchOnceSafe = false;
                DataCountdown.WarnCount = 0;
                return;
            }

            if (DataCountdown.ActivatedTick + ModBlocks.CountdownDuration <= currentTick)
            {
                if (DataCountdown.CanSwitchSafely || ModBlocks.CountdownForceSwitch)
                {
                    if (currentPlatformList != null)
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
