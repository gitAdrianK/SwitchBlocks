namespace SwitchBlocks.Behaviours.Dummy
{
    using Blocks.Dummy;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockPre" />.
    /// </summary>
    public class BehaviourPre : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourPre() { }

        /// <summary>Auto data.</summary>
        public DataAuto Auto { get; set; }

        /// <summary>Basic data.</summary>
        public DataBasic Basic { get; set; }

        /// <summary>Countdown data.</summary>
        public DataCountdown Countdown { get; set; }

        /// <summary>Jump data.</summary>
        public DataJump Jump { get; set; }

        // Documentation is false, higher numbers are run first!
        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioFirst;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (this.Auto != null)
            {
                this.Auto.CanSwitchSafely = true;
            }

            if (this.Basic != null)
            {
                this.Basic.CanSwitchSafely = true;
            }

            if (this.Countdown != null)
            {
                this.Countdown.CanSwitchSafely = true;
            }

            if (this.Jump != null)
            {
                this.Jump.CanSwitchSafely = true;
            }

            // Vanilla related gimmick.
            BehaviourPost.IsPlayerOnIce = false;
            BehaviourPost.IsPlayerOnSnow = false;
            BehaviourPost.IsPlayerOnWater = false;

            BehaviourPost.IsPlayerOnTypeSand = false;
            BehaviourPost.IsPlayerOnTypeSandUp = false;

            // Requested gimmick from another mod.
            BehaviourPost.IsPlayerOnMoveUp = false;
            BehaviourPost.IsPlayerOnInfinityJump = false;

            return true;
        }
    }
}
