using SwitchBlocks.Behaviours;
using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering auto platforms in the level.<br />
    /// Singleton.
    /// </summary>
    public class EntityAutoPlatforms : EntityPlatforms
    {
        private bool switchOnceSafe = false;
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
            TrySwitch();
        }

        private void TrySwitch()
        {
            if (BehaviourAutoPlatform.CanSwitchSafely && switchOnceSafe)
            {
                if (currentPlatformList != null)
                {
                    ModSounds.autoFlip?.PlayOneShot();
                }
                DataAuto.State = !DataAuto.State;
                switchOnceSafe = false;
                return;
            }
            if (DataAuto.RemainingTime <= 0.0f)
            {
                if (BehaviourAutoPlatform.CanSwitchSafely || ModBlocks.autoForceSwitch)
                {
                    if (currentPlatformList != null)
                    {
                        ModSounds.autoFlip?.PlayOneShot();
                    }
                    DataAuto.State = !DataAuto.State;
                }
                else
                {
                    switchOnceSafe = true;
                }
                DataAuto.RemainingTime = ModBlocks.autoDuration;
            }
        }
    }
}
