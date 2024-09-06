﻿using SwitchBlocks.Data;
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
            int adjustedTick = currentTick - DataAuto.ResetTick;
            if (IsActiveOnCurrentScreen)
            {
                TryWarn(adjustedTick);
            }
            TrySwitch(adjustedTick);
        }

        private void TryWarn(int adjustedTick)
        {
            if (ModSounds.AutoWarn == null || DataAuto.WarnCount == ModBlocks.AutoWarnCount)
            {
                return;
            }
            int warnAdjust = (ModBlocks.AutoWarnCount - DataAuto.WarnCount) * ModBlocks.AutoWarnDuration;
            int warnTick = (adjustedTick + warnAdjust) % ModBlocks.AutoDuration;
            if (warnTick == 0)
            {
                DataAuto.WarnCount++;
                ModSounds.AutoWarn.PlayOneShot();
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

            if (currentPlatformList != null)
            {
                ModSounds.AutoFlip?.PlayOneShot();
            }
            DataAuto.WarnCount = 0;
        }
    }
}
