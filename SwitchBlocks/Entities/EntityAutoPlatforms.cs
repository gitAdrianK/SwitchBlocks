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
            UpdateProgress(DataAuto.State, deltaTime, ModBlocks.AutoMultiplier);

            DataAuto.RemainingTime -= deltaTime * 0.5f;
            if (IsActiveOnCurrentScreen)
            {
                TryWarn();
            }
            TrySwitch();
        }

        private void TryWarn()
        {
            if (ModSounds.AutoWarn == null || DataAuto.WarnCount == ModBlocks.AutoWarnCount)
            {
                return;
            }
            if (DataAuto.RemainingTime <= ModBlocks.AutoWarnCount - DataAuto.WarnCount)
            {
                ModSounds.AutoWarn.PlayOneShot();
                DataAuto.WarnCount++;
            }
        }

        private void TrySwitch()
        {
            if (DataAuto.CanSwitchSafely && DataAuto.SwitchOnceSafe)
            {
                DataAuto.State = !DataAuto.State;
                DataAuto.SwitchOnceSafe = false;
                return;
            }
            if (DataAuto.RemainingTime <= 0.0f)
            {
                if (DataAuto.CanSwitchSafely || ModBlocks.AutoForceSwitch)
                {
                    DataAuto.State = !DataAuto.State;
                }
                else
                {
                    DataAuto.SwitchOnceSafe = true;
                }
                if (currentPlatformList != null)
                {
                    ModSounds.AutoFlip?.PlayOneShot();
                }
                DataAuto.RemainingTime = ModBlocks.AutoDuration;
                DataAuto.WarnCount = 0;
            }
        }
    }
}
