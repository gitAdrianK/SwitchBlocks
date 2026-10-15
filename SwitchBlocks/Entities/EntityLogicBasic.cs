namespace SwitchBlocks.Entities
{
    using Data;
    using Patches;
    using Settings;

    /// <summary>
    ///     Basic logic entity.
    /// </summary>
    public class EntityLogicBasic : EntityLogic<DataBasic>
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicBasic(SettingsBasic settings) : base(DataBasic.Instance)
            => this.UpdateSettings(settings);

        /// <summary>If the state is forced to switch regardless of player intersection.</summary>
        private bool ForceSwitch { get; set; }

        /// <summary>If the state can be switched on input.</summary>
        private bool CanSwitchOnPress { get; set; }

        /// <summary>Stores the previous input.</summary>
        private bool PreviousInput { get; set; }

        /// <summary>
        ///     Updates the settings from the given settings.
        /// </summary>
        /// <param name="settings"><see cref="SettingsBasic" />.</param>
        public void UpdateSettings(SettingsBasic settings)
        {
            this.Multiplier = settings.Multiplier;
            this.ForceSwitch = settings.ForceSwitch;
            this.CanSwitchOnPress = settings.CanSwitchOnPress;
            PatchControllerManager.CanSwitchOnPress = this.CanSwitchOnPress;
        }

        /// <summary>
        ///     Updates progress and tries to switch the state.
        /// </summary>
        /// <param name="deltaTime">deltaTime.</param>
        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);

            if (!this.PreviousInput && this.CanSwitchOnPress && PatchControllerManager.IsPressed)
            {
                this.Data.SwitchOnceSafe = true;
            }

            this.PreviousInput = PatchControllerManager.IsPressed;

            this.TrySwitch();
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
        /// </summary>
        private void TrySwitch()
        {
            if (!this.Data.SwitchOnceSafe || (!this.Data.CanSwitchSafely && !this.ForceSwitch))
            {
                return;
            }

            this.Data.SwitchOnceSafe = false;
            this.Data.State = !this.Data.State;
            this.Data.Tick = PatchAchievementManager.GetTick();
            ModSounds.BasicFlip?.PlayOneShot();
        }
    }
}
