namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour attached to the sequence A ice block.
    /// </summary>
    public class BehaviourSequenceIce : IBlockBehaviour
    {
        /// <summary>Sequence data.</summary>
        private DataSequence Data { get; }
        /// <inheritdoc/>
        public float BlockPriority => 2.0f;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourSequenceIce() => this.Data = DataSequence.Instance;

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
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceIceA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceD>();

            if (!this.IsPlayerOnBlock || BehaviourPost.IsPlayerOnIce)
            {
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockSequenceIceA)
                || type == typeof(BlockSequenceIceB)
                || type == typeof(BlockSequenceIceC)
                || type == typeof(BlockSequenceIceD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                if (this.Data.GetState(block.GroupId))
                {
                    BehaviourPost.IsPlayerOnIce = true;
                    break;
                }
            }

            return true;
        }
    }
}
