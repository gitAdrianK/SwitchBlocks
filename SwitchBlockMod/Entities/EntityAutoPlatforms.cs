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
            PlatformDictionary = Platform.GetPlatformsDictonary(ModStrings.AUTO);
            progress = DataAuto.Progress;
        }

        protected override void Update(float deltaTime)
        {
            UpdateProgress(DataAuto.State, deltaTime, ModBlocks.autoMultiplier);

            DataAuto.RemainingTime -= deltaTime * 0.5f;
            ThirdElapsed();
        }

        private void ThirdElapsed()
        {
            if (DataAuto.RemainingTime <= ModBlocks.autoDuration * 0.66 && !DataAuto.HasBlinkedOnce)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.autoBlink?.PlayOneShot();
                }
                DataAuto.HasBlinkedOnce = true;
                return;
            }
            if (DataAuto.RemainingTime <= ModBlocks.autoDuration * 0.33 && !DataAuto.HasBlinkedTwice)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.autoBlink?.PlayOneShot();
                }
                DataAuto.HasBlinkedTwice = true;
                return;
            }
            if (DataAuto.RemainingTime <= 0.0f)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.autoFlip?.PlayOneShot();
                }
                DataAuto.State = !DataAuto.State;
                DataAuto.RemainingTime = ModBlocks.autoDuration; ;
                DataAuto.HasBlinkedOnce = false;
                DataAuto.HasBlinkedTwice = false;
            }
        }
    }
}
