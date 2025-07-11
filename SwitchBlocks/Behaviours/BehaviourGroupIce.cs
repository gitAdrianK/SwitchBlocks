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
    ///     Behaviour attached to the <see cref="BlockGroupIceA" />.
    /// </summary>
    public class BehaviourGroupIce : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourGroupIce() => this.Groups = DataGroup.Instance.Groups;

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

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupIceA>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceB>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceC>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceD>();
            if (!this.IsPlayerOnBlock || BehaviourPost.IsPlayerOnIce)
            {
                return true;
            }

            var collided = new[]
            {
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceA>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceB>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceC>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceD>()
            }.SelectMany(block => block);
            var blocks = collided.Cast<IBlockGroupId>();

            foreach (var block in blocks)
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
