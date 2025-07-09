namespace SwitchBlocks.Entities
{
    using Data;
    using Patches;
    using Settings;

    /// <summary>
    ///     Auto logic entity.
    /// </summary>
    public class EntityLogicAuto : EntityLogic<DataAuto>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
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

        /// <summary>Duration the full cycle of on/off lasts for.</summary>
        private int DurationCycle { get; }

        /// <summary>Duration the on lasts for.</summary>
        private int DurationOn { get; }

        /// <summary>Duration the off lasts for.</summary>
        private int DurationOff { get; }

        /// <summary>Amount of warns played.</summary>
        private int WarnCount { get; }

        /// <summary>Duration warns are apart.</summary>
        private int WarnDuration { get; }

        /// <summary>If warn has been disabled for the state on.</summary>
        private bool WarnDisableOn { get; }

        /// <summary>If warn has been disabled for the state off.</summary>
        private bool WarnDisableOff { get; }

        /// <summary>If the state is forced to switch regardless of player intersection.</summary>
        private bool ForceSwitch { get; }

        /// <summary>
        ///     Updates progress, tries to play sounds and switch the state.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);

            var adjustedTick = (PatchAchievementManager.GetTick() + this.DurationCycle - this.Data.ResetTick) %
                               this.DurationCycle;
            this.TrySound(adjustedTick);
            this.TrySwitch(adjustedTick);
        }

        /// <summary>
        ///     Tries to make warn or flip sounds.
        /// </summary>
        /// <param name="adjustedTick">Tick adjusted for current cycle and tick reset.</param>
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

        /// <summary>
        ///     Plays the warn sound if it should do so.
        /// </summary>
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

        /// <summary>
        ///     Plays the flip sound.
        /// </summary>
        private void DoFlipSound()
        {
            this.Data.WarnCount = 0;
            if (!this.IsActiveOnCurrentScreen)
            {
                return;
            }

            ModSounds.AutoFlip?.PlayOneShot();
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
        /// </summary>
        /// <param name="adjustedTick">Tick adjusted for current cycle and tick reset.</param>
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
