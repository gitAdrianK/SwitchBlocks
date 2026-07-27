namespace SwitchBlocks.Entities
{
    using System.Collections.Generic;
    using Data;
    using Patches;
    using Settings;

    /// <summary>
    ///     Countdown logic entity.
    /// </summary>
    public class EntityLogicCountdown : EntityLogic<DataCountdown>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicCountdown(SettingsCountdown settings) : base(DataCountdown.Instance)
            => this.UpdateSettings(settings);

        /// <summary>Amount of warns played.</summary>
        private int WarnCount { get; set; }

        /// <summary>Duration warns are apart.</summary>
        private int WarnDuration { get; set; }

        /// <summary>If the state is forced to switch regardless of player intersection.</summary>
        private bool ForceSwitch { get; set; }

        ///<summary>If the single use countdown blocks reset when the timer ends.</summary>
        private bool SingleUseReset { get; set; }

        /// <summary>Ticks that play warn sounds.</summary>
        private HashSet<int> WarnTicks { get; set; }

        /// <summary>
        ///     Updates the settings from the given settings.
        /// </summary>
        /// <param name="settings"><see cref="SettingsCountdown" />.</param>
        public void UpdateSettings(SettingsCountdown settings)
        {
            this.Multiplier = settings.Multiplier;

            this.WarnCount = settings.WarnCount;
            this.WarnDuration = settings.WarnDuration;
            this.ForceSwitch = settings.ForceSwitch;
            this.SingleUseReset = settings.SingleUseReset;

            this.WarnTicks = new HashSet<int>();
            for (var i = 1; i <= this.WarnCount; i++)
            {
                this.WarnTicks.Add(this.WarnDuration * i);
            }
        }

        /// <summary>
        ///     Updates progress, tries to play sounds and switch the state.
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
            this.TryWarn(this.Data.DeactivatedTick - currentTick);
            this.TrySwitch(currentTick);
        }

        /// <summary>
        ///     Plays the warn sound if it should do so.
        /// </summary>
        /// <param name="adjustedTick">Tick adjusted for tick activated.</param>
        private void TryWarn(int adjustedTick)
        {
            if (!this.IsActiveOnCurrentScreen
                || ModSounds.CountdownWarn == null
                || this.Data.WarnCount == this.WarnCount
                || !this.WarnTicks.Contains(adjustedTick))
            {
                return;
            }

            ModSounds.CountdownWarn.PlayOneShot();
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
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

            if (this.Data.DeactivatedTick > currentTick)
            {
                return;
            }

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
