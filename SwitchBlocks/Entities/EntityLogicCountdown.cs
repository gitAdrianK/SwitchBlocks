namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;

    public class EntityLogicCountdown : EntityLogic<DataCountdown>
    {
        private int Duration { get; set; }
        private int WarnCount { get; set; }
        private int WarnDuration { get; set; }
        private bool ForceSwitch { get; set; }

        public EntityLogicCountdown() : base(DataCountdown.Instance, SettingsCountdown.Multiplier)
        {
            this.Duration = SettingsCountdown.Duration;
            this.WarnCount = SettingsCountdown.WarnCount;
            this.WarnDuration = SettingsCountdown.WarnDuration;
            this.ForceSwitch = SettingsCountdown.ForceSwitch;
        }

        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);

            if (!this.Data.State)
            {
                return;
            }

            var currentTick = AchievementManager.GetTicks();
            var adjustedTick = this.Duration - (currentTick - this.Data.ActivatedTick);
            if (this.IsActiveOnCurrentScreen)
            {
                this.TryWarn(adjustedTick);
            }
            this.TrySwitch(currentTick);
        }

        private void TryWarn(int adjustedTick)
        {
            if (ModSounds.CountdownWarn == null || this.Data.WarnCount == this.WarnCount)
            {
                return;
            }
            var warnAdjust = (this.WarnCount - this.Data.WarnCount) * this.WarnDuration;
            if (adjustedTick - warnAdjust == 0)
            {
                this.Data.WarnCount++;
                ModSounds.CountdownWarn.PlayOneShot();
            }
        }

        private void TrySwitch(int currentTick)
        {
            if (!this.Data.State)
            {
                return;
            }

            if (this.Data.CanSwitchSafely && this.Data.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }
                this.Data.State = false;
                this.Data.SwitchOnceSafe = false;
                this.Data.WarnCount = 0;
                return;
            }

            if (this.Data.ActivatedTick + this.Duration <= currentTick)
            {
                if (this.Data.CanSwitchSafely || this.ForceSwitch)
                {
                    if (this.IsActiveOnCurrentScreen)
                    {
                        ModSounds.CountdownFlip?.PlayOneShot();
                    }
                    this.Data.State = false;
                    this.Data.WarnCount = 0;
                }
                else
                {
                    this.Data.SwitchOnceSafe = true;
                }
            }
        }
    }
}
