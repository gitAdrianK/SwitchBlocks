namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockCountdownCustomDuration" />.
    /// </summary>
    public class BehaviourCountdownCustomDuration : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourCountdownCustomDuration(Direction leverDirections)
        {
            this.Data = DataCountdown.Instance;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Countdown data.</summary>
        private DataCountdown Data { get; }

        /// <summary>Lever directions.</summary>
        private Direction LeverDirections { get; set; }

        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioNormal;

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
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownCustomDuration>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockCountdownCustomDurationSolid>();
            this.IsPlayerOnBlock = collidingWithLever || collidingWithLeverSolid;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            IBlock block;
            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithLeverSolid)
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockCountdownCustomDurationSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }
            else
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockCountdownCustomDuration>().First();
            }

            var currentTick = PatchAchievementManager.GetTick();
            this.Data.ActivatedTick = currentTick;
            this.Data.DeactivatedTick = currentTick + ((IBlockDuration)block).Duration;

            if (this.Data.HasSwitched)
            {
                return true;
            }

            if (!this.Data.State)
            {
                ModSounds.CountdownFlip?.PlayOneShot();
            }

            this.Data.State = true;
            this.Data.HasSwitched = true;

            return true;
        }

        /// <summary>
        ///     Updates the directions a lever can be activated from the given directions.
        /// </summary>
        /// <param name="leverDirections">Directions a lever can be activated from.</param>
        public void UpdateDirections(Direction leverDirections) => this.LeverDirections = leverDirections;
    }
}
