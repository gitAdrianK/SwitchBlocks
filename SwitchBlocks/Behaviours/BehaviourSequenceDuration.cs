namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockSequenceA" />.
    /// </summary>
    public class BehaviourSequenceDuration : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSequenceDuration(int duration, Direction platformDirections)
        {
            var data = DataSequence.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Finished = data.Finished;
            this.Duration = duration;
            this.PlatformDirections = platformDirections;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active.</summary>
        private HashSet<int> Active { get; }

        private HashSet<int> Finished { get; }

        /// <summary>Duration.</summary>
        private int Duration { get; }

        /// <summary>Platform directions.</summary>
        private Direction PlatformDirections { get; }

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

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceA>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceB>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceC>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceD>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceA>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceB>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceC>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceIceD>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowC>()
                                   || advCollisionInfo.IsCollidingWith<BlockSequenceSnowD>();

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            var collided = new[]
            {
                advCollisionInfo.GetCollidedBlocks<BlockSequenceA>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceB>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceC>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceD>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceIceA>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceIceB>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceIceC>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceIceD>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceSnowA>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceSnowB>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceSnowC>(),
                advCollisionInfo.GetCollidedBlocks<BlockSequenceSnowD>()
            }.SelectMany(block => block);
            var blocks = collided.Cast<IBlockGroupId>();


            foreach (var block in blocks)
            {
                var groupId = block.GroupId;
                if (!this.Groups.TryGetValue(groupId, out var group)
                    || !group.State
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                        this.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                if (group.ActivatedTick == int.MaxValue)
                {
                    var tick = PatchAchievementManager.GetTick();
                    group.ActivatedTick = tick + this.Duration;
                    _ = this.Active.Add(groupId);
                    _ = this.Finished.Remove(groupId);
                }

                if (!this.Groups.TryGetValue(groupId + 1, out var nextGroup))
                {
                    continue;
                }

                nextGroup.ActivatedTick = int.MaxValue;
                _ = this.Active.Add(groupId + 1);
            }

            return true;
        }
    }
}
