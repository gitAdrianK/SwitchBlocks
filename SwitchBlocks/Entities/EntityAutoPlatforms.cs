using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Platforms;

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
            int adjustedTick = currentTick - DataAuto.ResetTick;
            TrySound(adjustedTick);
            TrySwitch(adjustedTick);
        }

        private void TrySound(int adjustedTick)
        {
            int soundAdjust = (ModBlocks.AutoWarnCount - DataAuto.WarnCount) * ModBlocks.AutoWarnDuration;
            int soundTick = (adjustedTick + soundAdjust) % ModBlocks.AutoDuration;
            if (soundTick != 0)
            {
                return;
            }
            if (ModBlocks.AutoWarnCount == DataAuto.WarnCount)
            {
                if (IsActiveOnCurrentScreen)
                {
                    ModSounds.AutoFlip?.PlayOneShot();
                }
                DataAuto.WarnCount = 0;
            }
            else
            {
                if (IsActiveOnCurrentScreen)
                {
                    ModSounds.AutoWarn?.PlayOneShot();
                }
                DataAuto.WarnCount++;
            }
        }

        private void TrySwitch(int adjustedTick)
        {
            bool currState = adjustedTick / ModBlocks.AutoDuration % 2 == 0;
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
        }
    }
}
