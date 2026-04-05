namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using Data;
    using Dummy;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockThresholdOn" />.
    /// </summary>
    public class BehaviourThresholdOn : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourThresholdOn() => this.Data = DataThreshold.Instance;

        /// <summary>Threshold data.</summary>
        private DataThreshold Data { get; }

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

            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockThresholdOn>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockThresholdIceOn>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockThresholdSnowOn>();
            var isOnWater = advCollisionInfo.IsCollidingWith<BlockThresholdWaterOn>();
            this.IsPlayerOnBlock = isOnBasic
                                   || isOnIce
                                   || isOnSnow
                                   || isOnWater;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (this.Data.State)
            {
                BehaviourPost.IsPlayerOnIce |= isOnIce;
                BehaviourPost.IsPlayerOnSnow |= isOnSnow;
                BehaviourPost.IsPlayerOnWater |= isOnWater;
            }
            else
            {
                if (this.Data.CanSwitchSafely)
                {
                    this.Data.CanSwitchSafely = !Intersecting.IsIntersectingBlocks(
                        behaviourContext,
                        typeof(BlockThresholdOn),
                        typeof(BlockThresholdIceOn),
                        typeof(BlockThresholdSnowOn));
                }
            }

            return true;
        }
    }
}
