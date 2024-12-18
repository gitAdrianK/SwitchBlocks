using Microsoft.Xna.Framework.Graphics;
using SwitchBlocks.Data;
using SwitchBlocks.Entities.Drawables;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using System.Threading.Tasks;

namespace SwitchBlocks.Entities
{
    /// <summary>
    /// Entity responsible for rendering countdown platforms in the level.
    /// </summary>
    public class EntityCountdownPlatforms : EntityDrawables<PlatformInOut>
    {
        public EntityCountdownPlatforms() : base(ModStrings.XML_PLATFORMS, ModStrings.COUNTDOWN) { }

        private void TryWarn(int adjustedTick)
        {
            if (ModSounds.CountdownWarn == null || DataCountdown.WarnCount == SettingsCountdown.WarnCount)
            {
                return;
            }
            int warnAdjust = (SettingsCountdown.WarnCount - DataCountdown.WarnCount) * SettingsCountdown.WarnDuration;
            if (adjustedTick - warnAdjust == 0)
            {
                DataCountdown.WarnCount++;
                ModSounds.CountdownWarn.PlayOneShot();
            }
        }

        protected override void EntityUpdate(float p_delta)
        {
            DataCountdown.Progress = UpdateProgressClamped(
                DataCountdown.State,
                DataCountdown.Progress,
                p_delta,
                SettingsCountdown.Multiplier);

            if (!DataCountdown.State)
            {
                return;
            }

            int currentTick = AchievementManager.GetTicks();
            int adjustedTick = SettingsCountdown.Duration - (currentTick - DataCountdown.ActivatedTick);
            if (IsActiveOnCurrentScreen)
            {
                TryWarn(adjustedTick);
            }
            TrySwitch(currentTick);
        }

        public override void EntityDraw(SpriteBatch spriteBatch)
        {
            Parallel.ForEach(currentDrawables, drawable =>
            {
                drawable.Draw(spriteBatch, DataCountdown.State, DataCountdown.Progress);
            });
        }

        private void TrySwitch(int currentTick)
        {
            if (!DataCountdown.State)
            {
                return;
            }

            if (DataCountdown.CanSwitchSafely && DataCountdown.SwitchOnceSafe)
            {
                if (IsActiveOnCurrentScreen)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }
                DataCountdown.State = false;
                DataCountdown.SwitchOnceSafe = false;
                DataCountdown.WarnCount = 0;
                return;
            }

            if (DataCountdown.ActivatedTick + SettingsCountdown.Duration <= currentTick)
            {
                if (DataCountdown.CanSwitchSafely || SettingsCountdown.ForceSwitch)
                {
                    if (IsActiveOnCurrentScreen)
                    {
                        ModSounds.CountdownFlip?.PlayOneShot();
                    }
                    DataCountdown.State = false;
                    DataCountdown.WarnCount = 0;
                }
                else
                {
                    DataCountdown.SwitchOnceSafe = true;
                }
            }
        }
    }
}
