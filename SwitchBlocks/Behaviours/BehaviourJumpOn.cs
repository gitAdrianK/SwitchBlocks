namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockJumpOn" />.
    /// </summary>
    public class BehaviourJumpOn : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourJumpOn() => this.Data = DataJump.Instance;

        /// <summary>Jump data.</summary>
        private DataJump Data { get; }

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

            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockJumpOn>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockJumpIceOn>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockJumpSnowOn>();
            var isOnWater = advCollisionInfo.IsCollidingWith<BlockJumpWaterOn>();

            var isOnInfinityJump = advCollisionInfo.IsCollidingWith<BlockJumpInfinityJumpOn>();

            this.IsPlayerOnBlock = isOnBasic || isOnIce || isOnSnow || isOnWater || isOnInfinityJump;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (this.Data.State)
            {
                BehaviourPost.IsPlayerOnIce |= isOnIce;
                BehaviourPost.IsPlayerOnSnow |= isOnSnow;
                BehaviourPost.IsPlayerOnWater |= isOnWater;

                BehaviourPost.IsPlayerOnInfinityJump |= isOnInfinityJump;
            }
            else
            {
                if (behaviourContext.BodyComp.IsOnGround)
                {
                    this.Data.SwitchOnceSafe = false;
                }

                if (this.Data.CanSwitchSafely)
                {
                    this.Data.CanSwitchSafely = !Intersecting.IsIntersectingBlocks(
                        behaviourContext,
                        typeof(BlockJumpOn),
                        typeof(BlockJumpIceOn),
                        typeof(BlockJumpSnowOn));
                }
            }

            return true;
        }
    }
}
