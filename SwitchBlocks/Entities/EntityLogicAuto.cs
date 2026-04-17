namespace SwitchBlocks.Entities
{
    using System.Collections.Generic;
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
        public EntityLogicAuto(SettingsAuto settings) : base(DataAuto.Instance)
            => this.UpdateSettings(settings);

        /// <summary>Duration the full cycle of on/off lasts for.</summary>
        private int DurationCycle { get; set; }

        /// <summary>Duration the on lasts for.</summary>
        private int DurationOn { get; set; }

        /// <summary>Duration the off lasts for.</summary>
        private int DurationOff { get; set; }

        /// <summary>Amount of warns played.</summary>
        private int WarnCount { get; set; }

        /// <summary>Duration warns are apart.</summary>
        private int WarnDuration { get; set; }

        /// <summary>If warn has been disabled for the state on.</summary>
        private bool WarnDisableOn { get; set; }

        /// <summary>If warn has been disabled for the state off.</summary>
        private bool WarnDisableOff { get; set; }

        /// <summary>If the state is forced to switch regardless of player intersection.</summary>
        private bool ForceSwitch { get; set; }

        /// <summary>Ticks that play warn sounds.</summary>
        private HashSet<int> WarnTicks { get; set; }

        /// <summary>Ticks that play flip sounds.</summary>
        private HashSet<int> FlipTicks { get; set; }

        /// <summary>
        ///     Updates the settings from the given settings.
        /// </summary>
        /// <param name="settings"><see cref="SettingsAuto" />.</param>
        public void UpdateSettings(SettingsAuto settings)
        {
            this.Multiplier = settings.Multiplier;

            this.DurationCycle = settings.DurationCycle;
            this.DurationOn = settings.DurationOn;
            this.DurationOff = settings.DurationOff;
            this.WarnCount = settings.WarnCount;
            this.WarnDuration = settings.WarnDuration;
            this.WarnDisableOn = settings.WarnDisableOn;
            this.WarnDisableOff = settings.WarnDisableOff;
            this.ForceSwitch = settings.ForceSwitch;

            this.FlipTicks = new HashSet<int> { settings.DurationOff, settings.DurationCycle - 1 };
            this.WarnTicks = new HashSet<int>();
            for (var i = 1; i <= this.WarnCount; i++)
            {
                this.WarnTicks.Add(this.DurationOff - (this.WarnDuration * i));
                this.WarnTicks.Add(this.DurationCycle - (this.WarnDuration * i));
            }
        }


        /// <summary>
        ///     Updates progress, tries to play sounds and switch the state.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);

            var adjustedTick = (PatchAchievementManager.GetTick() - this.Data.ResetTick + this.DurationCycle) %
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
            if (!this.IsActiveOnCurrentScreen)
            {
                return;
            }

            if (this.WarnTicks.Contains(adjustedTick))
            {
                this.DoWarnSound(adjustedTick);
                return;
            }

            if (this.FlipTicks.Contains(adjustedTick))
            {
                ModSounds.AutoFlip?.PlayOneShot();
            }
        }

        /// <summary>
        ///     Plays the warn sound if it should do so.
        /// </summary>
        private void DoWarnSound(int adjustedTick)
        {
            // The sound was disabled
            var currState = adjustedTick - this.DurationOn < 0;
            if ((currState && this.WarnDisableOn)
                || (!currState && this.WarnDisableOff))
            {
                return;
            }

            ModSounds.AutoWarn?.PlayOneShot();
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
        /// </summary>
        /// <param name="adjustedTick">Tick adjusted for current cycle and tick reset.</param>
        private void TrySwitch(int adjustedTick)
        {
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
