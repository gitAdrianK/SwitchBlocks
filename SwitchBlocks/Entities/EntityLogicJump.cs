namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Sequence logic entity.
    /// </summary>
    public class EntityLogicJump : EntityLogic<DataJump>
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        public EntityLogicJump() : base(DataJump.Instance, SettingsJump.Multiplier)
        {
        }

        /// <summary>
        /// Updates progress and tries to switch state.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);
            this.TrySwitch();
        }

        /// <summary>
        /// Tries to switch the state if it should do so.
        /// </summary>
        private void TrySwitch()
        {
            if (this.Data.CanSwitchSafely && this.Data.SwitchOnceSafe)
            {
                if (this.IsActiveOnCurrentScreen)
                {
                    ModSounds.JumpFlip?.PlayOneShot();
                }
                this.Data.State = !this.Data.State;
                this.Data.SwitchOnceSafe = false;
            }
        }
    }
}
