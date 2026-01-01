namespace SwitchBlocks.Entities
{
    using Data;
    using JumpKing.Controller;
    using JumpKing.Player;
    using Patches;
    using Settings;

    /// <summary>
    ///     Sequence logic entity.
    /// </summary>
    public class EntityLogicJump : EntityLogic<DataJump>
    {
        /// <summary>If the player can trigger another switch in air.</summary>
        private bool CanJumpInAir { get; }

        /// <summary>The players body comp.</summary>
        private BodyComp Body { get; }

        /// <summary>
        ///     Ctor.
        /// </summary>
        public EntityLogicJump(SettingsJump settings, PlayerEntity player) : base(DataJump.Instance, settings.Multiplier)
        {
            this.CanJumpInAir = settings.CanJumpInAir;
            this.Body = player.m_body;
        }

        /// <summary>
        ///     Updates progress and tries to switch state.
        /// </summary>
        /// <param name="deltaTime"></param>
        protected override void Update(float deltaTime)
        {
            if (this.CanJumpInAir && !this.Body.IsOnGround)
            {
                var padState = ControllerManager.instance.GetPressedPadState();
                if (padState.jump)
                {
                    this.Data.SwitchOnceSafe = true;
                }
            }

            this.UpdateProgress(this.Data.State, deltaTime);
            this.TrySwitch();
        }

        /// <summary>
        ///     Tries to switch the state if it should do so.
        /// </summary>
        private void TrySwitch()
        {
            if (!this.Data.CanSwitchSafely || !this.Data.SwitchOnceSafe)
            {
                return;
            }

            if (this.IsActiveOnCurrentScreen)
            {
                ModSounds.JumpFlip?.PlayOneShot();
            }

            this.Data.State = !this.Data.State;
            this.Data.SwitchOnceSafe = false;
            this.Data.Tick = PatchAchievementManager.GetTick();
        }
    }
}
