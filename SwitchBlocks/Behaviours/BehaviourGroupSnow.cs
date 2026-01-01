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
    ///     Behaviour attached to the <see cref="BlockGroupSnowA" />.
    /// </summary>
    public class BehaviourGroupSnow : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourGroupSnow() => this.Groups = DataGroup.Instance.Groups;

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

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupSnowA>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowB>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowC>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowD>();
            if (!this.IsPlayerOnBlock || BehaviourPost.IsPlayerOnSnow)
            {
                return true;
            }

            var collided = new[]
            {
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowA>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowB>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowC>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowD>(),
            }.SelectMany(block => block);
            var blocks = collided.Cast<IBlockGroupId>();

            foreach (var block in blocks)
            {
                if (!this.Groups.TryGetValue(block.GroupId, out var group) || !group.State)
                {
                    continue;
                }

                BehaviourPost.IsPlayerOnSnow = true;
                break;
            }

            return true;
        }
    }
}
