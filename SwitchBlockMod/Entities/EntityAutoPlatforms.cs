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
            DataAuto.Progress = progress;
            instance = null;
        }

        private EntityAutoPlatforms()
        {
            PlatformDictionary = Platform.GetPlatformsDictonary("auto");
            progress = DataAuto.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataAuto.State, deltaTime);

            DataAuto.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();
        }
        private void ThirdElapsed()
        {
            if (DataAuto.RemainingTime <= ModBlocks.AUTO_DURATION * 0.66 && !DataAuto.HasBlinkedOnce)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.AUTO_BLINK?.Play();
                }
                DataAuto.HasBlinkedOnce = true;
                return;
            }
            if (DataAuto.RemainingTime <= ModBlocks.AUTO_DURATION * 0.33 && !DataAuto.HasBlinkedTwice)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.AUTO_BLINK?.Play();
                }
                DataAuto.HasBlinkedTwice = true;
                return;
            }
            if (DataAuto.RemainingTime <= 0.0f)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.AUTO_FLIP?.Play();
                }
                DataAuto.State = !DataAuto.State;
                DataAuto.RemainingTime = ModBlocks.AUTO_DURATION; ;
                DataAuto.HasBlinkedOnce = false;
                DataAuto.HasBlinkedTwice = false;
            }
        }
    }
}
