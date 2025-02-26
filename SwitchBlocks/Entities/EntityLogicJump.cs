namespace SwitchBlocks.Entities
{
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;

    public class EntityLogicJump : EntityLogic<DataJump>
    {
        public EntityLogicJump() : base(DataJump.Instance, SettingsJump.Multiplier)
        {
        }

        protected override void Update(float deltaTime)
        {
            this.UpdateProgress(this.Data.State, deltaTime);
            this.TrySwitch();
        }

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
