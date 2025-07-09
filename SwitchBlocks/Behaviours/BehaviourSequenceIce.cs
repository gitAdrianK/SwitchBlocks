namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockSequenceIceA" />.
    /// </summary>
    public class BehaviourSequenceIce : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSequenceIce() => this.Groups = DataSequence.Instance.Groups;

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

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
                if (!this.Groups.TryGetValue(block.GroupId, out var group) || !group.State)
                {
                    continue;
                }

                BehaviourPost.IsPlayerOnIce = true;
                break;
            }

            return true;
        }
    }
}
