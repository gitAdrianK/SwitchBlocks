namespace SwitchBlocks.Entities
{
    using System.Threading.Tasks;
    using Microsoft.Xna.Framework.Graphics;
    using SwitchBlocks.Data;
    using SwitchBlocks.Entities.Drawables;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Entity responsible for rendering countdown platforms in the level.
    /// </summary>
    public class EntityCountdownPlatforms : EntityDrawables<PlatformInOut>
    {
        public EntityCountdownPlatforms() : base(ModConsts.XML_PLATFORMS, ModConsts.COUNTDOWN) { }

        private void TryWarn(int adjustedTick)
        {
            if (ModSounds.CountdownWarn == null || DataCountdown.WarnCount == SettingsCountdown.WarnCount)
            {
                return;
            }
            var warnAdjust = (SettingsCountdown.WarnCount - DataCountdown.WarnCount) * SettingsCountdown.WarnDuration;
            if (adjustedTick - warnAdjust == 0)
            {
                DataCountdown.WarnCount++;
                // The sound was disabled
                if (DataCountdown.State)
                {
                    if (SettingsCountdown.WarnDisableOn)
                    {
                        return;
                    }
                }
                else
                {
                    if (SettingsCountdown.WarnDisableOff)
                    {
                        return;
                    }
                }
                ModSounds.CountdownWarn.PlayOneShot();
            }
        }

        protected override void EntityUpdate(float p_delta)
        {
            DataCountdown.Progress = this.UpdateProgressClamped(
                DataCountdown.State,
                DataCountdown.Progress,
                p_delta,
                SettingsCountdown.Multiplier);

            if (!DataCountdown.State)
            {
                return;
            }

            var currentTick = AchievementManager.GetTicks();
            var adjustedTick = SettingsCountdown.Duration - (currentTick - DataCountdown.ActivatedTick);
            if (this.IsActiveOnCurrentScreen)
            {
                this.TryWarn(adjustedTick);
            }
            this.TrySwitch(currentTick);
        }

        public override void EntityDraw(SpriteBatch spriteBatch) => _ = Parallel.ForEach(this.CurrentDrawables, drawable
            => drawable.Draw(
                spriteBatch,
                DataCountdown.State,
                DataCountdown.Progress));

        private void TrySwitch(int currentTick)
        {
            if (!DataCountdown.State)
            {
                return;
            }

            if (DataCountdown.CanSwitchSafely && DataCountdown.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
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
                    if (this.IsActiveOnCurrentScreen)
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
