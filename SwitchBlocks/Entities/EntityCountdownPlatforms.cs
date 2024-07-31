using SwitchBlocks.Behaviours;
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
        private bool switchOnceSafe = false;
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
            TrySwitch();

        }

        private void TrySwitch()
        {
            if (BehaviourCountdownPlatform.CanSwitchSafely && switchOnceSafe)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.countdownFlip?.PlayOneShot();
                }
                DataCountdown.State = false;
                DataCountdown.RemainingTime = ModBlocks.countdownDuration;
                switchOnceSafe = false;
                return;
            }
            if (DataCountdown.RemainingTime <= 0.0f)
            {
                if (BehaviourCountdownPlatform.CanSwitchSafely || ModBlocks.countdownForceSwitch)
                {
                    if (currentPlatformList != null)
                    {
                        ModSounds.countdownFlip?.PlayOneShot();
                    }
                    DataCountdown.State = false;
                }
                else
                {
                    switchOnceSafe = true;
                }
                DataCountdown.RemainingTime = ModBlocks.countdownDuration;
            }
        }
    }
}
