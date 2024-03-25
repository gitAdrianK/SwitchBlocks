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
            instance = null;
        }

        private EntityCountdownPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("countdown");
        }

        protected override void Update(float deltaTime)
        {
            if (!UpdateCurrentScreen())
            {
                return;
            }

            UpdateProgress(DataCountdown.State, deltaTime);

            if (!DataCountdown.State)
            {
                return;
            }
            DataCountdown.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();

        }

        private void ThirdElapsed()
        {
            if (DataCountdown.RemainingTime <= ModBlocks.COUNTDOWN_DURATION * 0.66 && !DataCountdown.HasBlinkedOnce)
            {
                if (ModSounds.COUNTDOWN_BLINK != null)
                {
                    ModSounds.COUNTDOWN_BLINK.Play();
                }
                DataCountdown.HasBlinkedOnce = true;
                return;
            }
            if (DataCountdown.RemainingTime <= ModBlocks.COUNTDOWN_DURATION * 0.33 && !DataCountdown.HasBlinkedTwice)
            {
                if (ModSounds.COUNTDOWN_BLINK != null)
                {
                    ModSounds.COUNTDOWN_BLINK.Play();
                }
                DataCountdown.HasBlinkedTwice = true;
                return;
            }
            if (DataCountdown.RemainingTime <= 0.0f)
            {
                if (ModSounds.COUNTDOWN_FLIP != null)
                {
                    ModSounds.COUNTDOWN_FLIP.Play();
                }
                DataCountdown.State = false;
                DataCountdown.RemainingTime = ModBlocks.COUNTDOWN_DURATION; ;
                DataCountdown.HasBlinkedOnce = false;
                DataCountdown.HasBlinkedTwice = false;
            }
        }
    }
}
