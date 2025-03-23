namespace SwitchBlocks.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Data;

    /// <summary>
    /// Behaviour attached to the pre block.
    /// </summary>
    public class BehaviourPre : IBlockBehaviour
    {
        /// <summary>Auto data.</summary>
        private DataAuto Auto { get; }
        /// <summary>Countdown data.</summary>
        private DataCountdown Countdown { get; }
        /// <summary>Jump data.</summary>
        private DataJump Jump { get; }
        // Documentation is false, higher numbers are run first!
        /// <inheritdoc/>
        public float BlockPriority => ModConsts.PRIO_FIRST;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourPre()
        {
            this.Auto = DataAuto.Instance;
            this.Countdown = DataCountdown.Instance;
            this.Jump = DataJump.Instance;
        }

        /// <inheritdoc/>
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc/>
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc/>
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc/>
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc/>
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc/>
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            this.Auto.CanSwitchSafely = true;
            this.Countdown.CanSwitchSafely = true;
            this.Jump.CanSwitchSafely = true;

            BehaviourPost.IsPlayerOnIce = false;
            BehaviourPost.IsPlayerOnSnow = false;
            BehaviourPost.IsPlayerOnSand = false;

            return true;
        }
    }
}
