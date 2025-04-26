namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Patches;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Countdown logic entity.
    /// </summary>
    public class EntityLogicCountdown : EntityLogic<DataCountdown>
    {
        /// <summary>Duration the state switch lasts for.</summary>
        private int Duration { get; set; }
        /// <summary>Amount of warns played.</summary>
        private int WarnCount { get; set; }
        /// <summary>Duration warns are apart.</summary>
        private int WarnDuration { get; set; }
        /// <summary>If the state is forced to switch regardless of player intersection.</summary>
        private bool ForceSwitch { get; set; }
        ///<summary>If the single use countdown blocks reset when the timer ends.</summary>
        private bool SingleUseReset { get; set; }

        /// <summary>
        /// Ctor.
        /// </summary>
        public EntityLogicCountdown() : base(DataCountdown.Instance, SettingsCountdown.Multiplier)
        {
            this.Duration = SettingsCountdown.Duration;
            this.WarnCount = SettingsCountdown.WarnCount;
            this.WarnDuration = SettingsCountdown.WarnDuration;
            this.ForceSwitch = SettingsCountdown.ForceSwitch;
            this.SingleUseReset = SettingsCountdown.SingleUseReset;
        }

        /// <summary>
        /// Updates progress, tries to play sounds and switch the state.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);

            if (!this.Data.State)
            {
                return;
            }

            var currentTick = PatchAchievementManager.GetTick();
            if (this.IsActiveOnCurrentScreen)
            {
                this.TryWarn(this.Duration - (currentTick - this.Data.ActivatedTick));
            }
            this.TrySwitch(currentTick);
        }

        /// <summary>
        /// Plays the warn sound if it should do so.
        /// </summary>
        /// <param name="adjustedTick">Tick adjusted for tick activated.</param>
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

        /// <summary>
        /// Tries to switch the state if it should do so.
        /// </summary>
        /// <param name="currentTick">Tick adjusted for tick activated.</param>
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
                if (this.SingleUseReset)
                {
                    this.Data.Touched.Clear();
                }
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
                    if (this.SingleUseReset)
                    {
                        this.Data.Touched.Clear();
                    }
                }
                else
                {
                    this.Data.SwitchOnceSafe = true;
                }
            }
        }
    }
}
