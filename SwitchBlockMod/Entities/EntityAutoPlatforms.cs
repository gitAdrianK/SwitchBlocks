using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;

namespace SwitchBlocksMod.Entities
{
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
            instance = null;
        }

        private EntityAutoPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("auto");
        }

        protected override void Update(float deltaTime)
        {
            if (!UpdateCurrentScreen())
            {
                return;
            }

            if (progress != 1.0f && DataAuto.State)
            {
                progress += deltaTime;
                if (progress >= 1.0f)
                {
                    progress = 1.0f;
                }
            }
            else if (progress != 0.0f && !DataAuto.State)
            {
                progress -= 5.0f * deltaTime;
                if (progress <= 0.0f)
                {
                    progress = 0.0f;
                }
            }

            DataAuto.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();
        }
        private void ThirdElapsed()
        {
            if (DataAuto.RemainingTime <= ModBlocks.AUTO_DURATION * 0.66 && !DataAuto.HasBlinkedOnce)
            {
                if (ModSounds.AUTO_BLINK != null)
                {
                    ModSounds.AUTO_BLINK.Play();
                }
                DataAuto.HasBlinkedOnce = true;
                return;
            }
            if (DataAuto.RemainingTime <= ModBlocks.AUTO_DURATION * 0.33 && !DataAuto.HasBlinkedTwice)
            {
                if (ModSounds.AUTO_BLINK != null)
                {
                    ModSounds.AUTO_BLINK.Play();
                }
                DataAuto.HasBlinkedTwice = true;
                return;
            }
            if (DataAuto.RemainingTime <= 0.0f)
            {
                if (ModSounds.AUTO_FLIP != null)
                {
                    ModSounds.AUTO_FLIP.Play();
                }
                DataAuto.State = !DataAuto.State;
                DataAuto.RemainingTime = ModBlocks.AUTO_DURATION; ;
                DataAuto.HasBlinkedOnce = false;
                DataAuto.HasBlinkedTwice = false;
            }
        }
    }
}
