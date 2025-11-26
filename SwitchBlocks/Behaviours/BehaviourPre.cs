namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Setups;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockPre" />.
    /// </summary>
    public class BehaviourPre : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourPre()
        {
            if (SetupAuto.IsUsed)
            {
                this.Auto = DataAuto.Instance;
            }

            if (SetupCountdown.IsUsed)
            {
                this.Countdown = DataCountdown.Instance;
            }

            if (SetupJump.IsUsed)
            {
                this.Jump = DataJump.Instance;
            }
        }

        /// <summary>Auto data.</summary>
        private DataAuto Auto { get; }

        /// <summary>Countdown data.</summary>
        private DataCountdown Countdown { get; }

        /// <summary>Jump data.</summary>
        private DataJump Jump { get; }

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
            if (!(this.Auto is null))
            {
                this.Auto.CanSwitchSafely = true;
            }

            if (!(this.Countdown is null))
            {
                this.Countdown.CanSwitchSafely = true;
            }

            if (!(this.Jump is null))
            {
                this.Jump.CanSwitchSafely = true;
            }

            // Vanilla related gimmick.
            BehaviourPost.IsPlayerOnIce = false;
            BehaviourPost.IsPlayerOnSnow = false;
            BehaviourPost.IsPlayerOnWater = false;

            BehaviourPost.IsPlayerOnSand = false;
            BehaviourPost.IsPlayerOnSandUp = false;

            // Requested gimmick from another mod.
            BehaviourPost.IsPlayerOnMoveUp = false;
            BehaviourPost.IsPlayerOnInfinityJump = false;

            return true;
        }
    }
}
