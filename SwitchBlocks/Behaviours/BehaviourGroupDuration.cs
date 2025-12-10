namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Collections.Specialized;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockGroupA" /> should a duration be set.
    /// </summary>
    public class BehaviourGroupDuration : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourGroupDuration(int duration, BitVector32 platformDirections)
        {
            var data = DataGroup.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Touched = data.Touched;
            this.Duration = duration;
            this.PlatformDirections = platformDirections;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active./// </summary>
        private HashSet<int> Active { get; }

        /// <summary>Cached IDs considered touched./// </summary>
        private HashSet<int> Touched { get; }

        /// <summary>Duration.</summary>
        private int Duration { get; }

        /// <summary>Platform directions.</summary>
        private BitVector32 PlatformDirections { get; }

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

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupA>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupB>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupC>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupD>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceA>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceB>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceC>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupIceD>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowA>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowB>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowC>()
                                   || advCollisionInfo.IsCollidingWith<BlockGroupSnowD>();

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            var tick = PatchAchievementManager.GetTick();
            var collided = new[]
            {
                advCollisionInfo.GetCollidedBlocks<BlockGroupA>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupB>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupC>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupD>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceA>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceB>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceC>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupIceD>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowA>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowB>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowC>(),
                advCollisionInfo.GetCollidedBlocks<BlockGroupSnowD>()
            }.SelectMany(block => block);
            var blocks = collided.Cast<IBlockGroupId>();

            foreach (var block in blocks)
            {
                var groupId = block.GroupId;
                if (!this.Groups.TryGetValue(groupId, out var group))
                {
                    continue;
                }

                if (!group.State
                    || this.Touched.Contains(groupId)
                    || !Directions.ResolveCollisionDirection(
                        behaviourContext,
                        this.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                group.ActivatedTick = tick + this.Duration;
                _ = this.Active.Add(groupId);
                _ = this.Touched.Add(groupId);
            }

            return true;
        }
    }
}
