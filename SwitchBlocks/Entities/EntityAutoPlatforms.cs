using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using System;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering auto platforms in the level.
    /// </summary>
    public class EntityAutoPlatforms : EntityDrawables<PlatformInOut>
    {
        public EntityAutoPlatforms() : base(ModStrings.XML_PLATFORMS, ModStrings.AUTO) { }

        protected override void EntityUpdate(float p_delta)
        {
            DataAuto.Progress = UpdateProgressClamped(
                DataAuto.State,
                DataAuto.Progress,
                p_delta,
                SettingsAuto.Multiplier);
            int currentTick = AchievementManager.GetTicks();
            int adjustedTick = ((currentTick + SettingsAuto.DurationCycle) - DataAuto.ResetTick) % SettingsAuto.DurationCycle;
            TrySound(adjustedTick);
            TrySwitch(adjustedTick);
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(spriteBatch, DataAuto.State, DataAuto.Progress);
            });
        }

        private void TrySound(int adjustedTick)
        {
            adjustedTick += SettingsAuto.DurationOff * Convert.ToInt32(DataAuto.State);
            int soundAdjust = (SettingsAuto.WarnCount - DataAuto.WarnCount) * SettingsAuto.WarnDuration;
            int soundTick = (adjustedTick + soundAdjust) % SettingsAuto.DurationCycle;
            // Its not yet time to make a sound
            if (soundTick != 0)
            {
                return;
            }
            // Check which sound is to be played
            if (SettingsAuto.WarnCount == DataAuto.WarnCount)
            {
                DoFlipSound();
            }
            else
            {
                DoWarnSound();
            }
        }

        private void DoWarnSound()
        {
            DataAuto.WarnCount++;
            if (!IsActiveOnCurrentScreen)
            {
                return;
            }
            // The sound was disabled
            if (DataAuto.State)
            {
                if (SettingsAuto.WarnDisableOn)
                {
                    return;
                }
            }
            else
            {
                if (SettingsAuto.WarnDisableOff)
                {
                    return;
                }
            }
            ModSounds.AutoWarn?.PlayOneShot();
        }

        private void DoFlipSound()
        {
            DataAuto.WarnCount = 0;
            if (IsActiveOnCurrentScreen)
            {
                ModSounds.AutoFlip?.PlayOneShot();
            }
        }

        private void TrySwitch(int adjustedTick)
        {
            // I think its < but it could be <=
            bool currState = adjustedTick - SettingsAuto.DurationOn < 0;
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

            if (DataAuto.CanSwitchSafely || SettingsAuto.ForceSwitch)
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
