namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockBasicOff" />.
    /// </summary>
    public class BehaviourBasicOff : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourBasicOff() => this.Data = DataBasic.Instance;

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

            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockBasicOff>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockBasicIceOff>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockBasicSnowOff>();
            var isOnMoveUp = advCollisionInfo.IsCollidingWith<BlockBasicMoveUpOff>();
            this.IsPlayerOnBlock = !this.Data.State && (isOnBasic || isOnIce || isOnSnow || isOnMoveUp);
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (this.Data.State)
            {
                return true;
            }

            BehaviourPost.IsPlayerOnIce |= isOnIce;
            BehaviourPost.IsPlayerOnSnow |= isOnSnow;

            BehaviourPost.IsPlayerOnMoveUp |= isOnMoveUp;

            return true;
        }
    }
}
