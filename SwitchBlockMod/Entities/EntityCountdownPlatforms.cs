using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;

namespace SwitchBlocksMod.Entities
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
            UpdateProgress(DataCountdown.State, deltaTime, ModBlocks.countdownMultiplier);

            if (!DataCountdown.State)
            {
                return;
            }
            DataCountdown.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();

        }

        private void ThirdElapsed()
        {
            if (DataCountdown.RemainingTime <= ModBlocks.countdownDuration * 0.66 && !DataCountdown.HasBlinkedOnce)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.countdownBlink?.Play();
                }
                DataCountdown.HasBlinkedOnce = true;
                return;
            }
            if (DataCountdown.RemainingTime <= ModBlocks.countdownDuration * 0.33 && !DataCountdown.HasBlinkedTwice)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.countdownBlink?.Play();
                }
                DataCountdown.HasBlinkedTwice = true;
                return;
            }
            if (DataCountdown.RemainingTime <= 0.0f)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.countdownFlip?.Play();
                }
                DataCountdown.State = false;
                DataCountdown.RemainingTime = ModBlocks.countdownDuration; ;
                DataCountdown.HasBlinkedOnce = false;
                DataCountdown.HasBlinkedTwice = false;
            }
        }
    }
}
