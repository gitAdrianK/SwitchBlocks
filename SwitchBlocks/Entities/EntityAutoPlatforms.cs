using SwitchBlocks.Data;
using SwitchBlocks.Patching;
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

            int currentTick = AchievementManager.GetTicks();
            if (IsActiveOnCurrentScreen)
            {
                TryWarn(currentTick);
            }
            TrySwitch(currentTick);
        }

        private void TryWarn(int currentTick)
        {
            if (ModSounds.AutoWarn == null || DataAuto.WarnCount == ModBlocks.AutoWarnCount)
            {
                return;
            }
            /*
            if (DataAuto.RemainingTime <= (ModBlocks.AutoWarnCount - DataAuto.WarnCount) * ModBlocks.AutoWarnDuration)
            {
                ModSounds.AutoWarn.PlayOneShot();
                DataAuto.WarnCount++;
            }
            */
        }

        private void TrySwitch(int currentTick)
        {
            bool currState = (currentTick - DataAuto.ResetTick) / ModBlocks.AutoDuration % 2 == 0;
            if (DataAuto.State == currState)
            {
                return;
            }

            if (DataAuto.CanSwitchSafely && DataAuto.SwitchOnceSafe)
            {
                DataAuto.State = currState;
                DataAuto.SwitchOnceSafe = false;
                return;
            }

            if (DataAuto.CanSwitchSafely || ModBlocks.AutoForceSwitch)
            {
                DataAuto.State = currState;
            }
            else
            {
                DataAuto.SwitchOnceSafe = true;
            }

            if (currentPlatformList != null)
            {
                ModSounds.AutoFlip?.PlayOneShot();
            }
            DataAuto.WarnCount = 0;

        }
    }
}
