namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;

    public class EntityLogicAuto : EntityLogic<DataAuto>
    {
        private int DurationCycle { get; set; }
        private int DurationOn { get; set; }
        private int DurationOff { get; set; }
        private int WarnCount { get; set; }
        private int WarnDuration { get; set; }
        private bool WarnDisableOn { get; set; }
        private bool WarnDisableOff { get; set; }
        private bool ForceSwitch { get; set; }

        public EntityLogicAuto() : base(DataAuto.Instance, SettingsAuto.Multiplier)
        {
            this.DurationCycle = SettingsAuto.DurationCycle;
            this.DurationOn = SettingsAuto.DurationOn;
            this.DurationOff = SettingsAuto.DurationOff;
            this.WarnCount = SettingsAuto.WarnCount;
            this.WarnDuration = SettingsAuto.WarnDuration;
            this.WarnDisableOn = SettingsAuto.WarnDisableOn;
            this.WarnDisableOff = SettingsAuto.WarnDisableOff;
            this.ForceSwitch = SettingsAuto.ForceSwitch;
        }

        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);

            var currentTick = AchievementManager.GetTicks();
            var adjustedTick = (currentTick + this.DurationCycle - this.Data.ResetTick) % this.DurationCycle;
            this.TrySound(adjustedTick);
            this.TrySwitch(adjustedTick);
        }

        private void TrySound(int adjustedTick)
        {
            if (this.Data.State)
            {
                adjustedTick += this.DurationOff;
            }
            var soundAdjust = (this.WarnCount - this.Data.WarnCount) * this.WarnDuration;
            var soundTick = (adjustedTick + soundAdjust) % this.DurationCycle;
            // Its not yet time to make a sound
            if (soundTick != 0)
            {
                return;
            }
            // Check which sound is to be played
            if (this.WarnCount == this.Data.WarnCount)
            {
                this.DoFlipSound();
            }
            else
            {
                this.DoWarnSound();
            }
        }

        private void DoWarnSound()
        {
            this.Data.WarnCount++;
            if (!this.IsActiveOnCurrentScreen)
            {
                return;
            }
            // The sound was disabled
            if (this.Data.State)
            {
                if (this.WarnDisableOn)
                {
                    return;
                }
            }
            else
            {
                if (this.WarnDisableOff)
                {
                    return;
                }
            }
            ModSounds.AutoWarn?.PlayOneShot();
        }

        private void DoFlipSound()
        {
            this.Data.WarnCount = 0;
            if (!this.IsActiveOnCurrentScreen)
            {
                return;
            }
            ModSounds.AutoFlip?.PlayOneShot();
        }

        private void TrySwitch(int adjustedTick)
        {
            // I think its < but it could be <=
            var currState = adjustedTick - this.DurationOn < 0;
            if (this.Data.State == currState)
            {
                return;
            }

            if (this.Data.CanSwitchSafely && this.Data.SwitchOnceSafe)
            {
                this.Data.State = currState;
                this.Data.SwitchOnceSafe = false;
                return;
            }


            if (this.Data.CanSwitchSafely || this.ForceSwitch)
            {
                this.Data.State = currState;
            }
            else
            {
                this.Data.SwitchOnceSafe = true;
            }
        }
    }
}
