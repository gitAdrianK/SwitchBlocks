namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour attached to the sequence A snow block.
    /// </summary>
    public class BehaviourSequenceSnow : IBlockBehaviour
    {
        /// <summary>Cached mappings of <see cref="BlockGroup"/>s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; set; }
        /// <inheritdoc/>
        public float BlockPriority => ModConsts.PRIO_NORMAL;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourSequenceSnow() => this.Groups = DataSequence.Instance.Groups;

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
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowD>();

            if (!this.IsPlayerOnBlock || BehaviourPost.IsPlayerOnSnow)
            {
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockSequenceSnowA)
                || type == typeof(BlockSequenceSnowB)
                || type == typeof(BlockSequenceSnowC)
                || type == typeof(BlockSequenceSnowD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                if (!this.Groups.TryGetValue(block.GroupId, out var group))
                {
                    continue;
                }
                if (group.State)
                {
                    BehaviourPost.IsPlayerOnSnow = true;
                    break;
                }
            }

            return true;
        }
    }
}
