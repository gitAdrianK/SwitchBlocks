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
    ///     Behaviour attached to the <see cref="BlockCountdownLever" />.
    /// </summary>
    public class BehaviourCountdownLever : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourCountdownLever(Direction leverDirections)
        {
            this.Data = DataCountdown.Instance;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Countdown data.</summary>
        private DataCountdown Data { get; }

        /// <summary>Lever directions.</summary>
        private Direction LeverDirections { get; }

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

            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownLever>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockCountdownLeverSolid>();
            this.IsPlayerOnBlock = collidingWithLever || collidingWithLeverSolid;

            if (!this.IsPlayerOnBlock)
            {
                this.Data.HasSwitched = false;
                return true;
            }

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithLeverSolid)
            {
                var block = advCollisionInfo.GetCollidedBlocks<BlockCountdownLeverSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }

            this.Data.ActivatedTick = PatchAchievementManager.GetTick();

            if (this.Data.HasSwitched)
            {
                return true;
            }

            if (!this.Data.State)
            {
                ModSounds.CountdownFlip?.PlayOneShot();
            }

            this.Data.HasSwitched = true;
            this.Data.State = true;

            return true;
        }
    }
}
