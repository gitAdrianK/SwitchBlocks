namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockBasicOn" />.
    /// </summary>
    public class BehaviourBasicOn : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourBasicOn() => this.Data = DataBasic.Instance;

        /// <summary>Basic data.</summary>
        private DataBasic Data { get; }

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
            if (advCollisionInfo is null)
            {
                return true;
            }

            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockBasicOn>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockBasicIceOn>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockBasicSnowOn>();
            var isOnWater = advCollisionInfo.IsCollidingWith<BlockBasicWaterOn>();
            var isOnInfinityJump = advCollisionInfo.IsCollidingWith<BlockBasicInfinityJumpOn>();

            var isOnMoveUp = advCollisionInfo.IsCollidingWith<BlockBasicMoveUpOn>();

            this.IsPlayerOnBlock = this.Data.State && (isOnBasic || isOnIce || isOnSnow || isOnWater || isOnMoveUp || isOnInfinityJump);
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (!this.Data.State)
            {
                return true;
            }

            BehaviourPost.IsPlayerOnIce |= isOnIce;
            BehaviourPost.IsPlayerOnSnow |= isOnSnow;
            BehaviourPost.IsPlayerOnWater |= isOnWater;

            BehaviourPost.IsPlayerOnMoveUp |= isOnMoveUp;
            BehaviourPost.IsPlayerOnInfinityJump |= isOnInfinityJump;

            return true;
        }
    }
}
