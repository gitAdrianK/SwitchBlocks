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
    using Settings;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockGroupA" /> should no duration be set.
    /// </summary>
    public class BehaviourGroupLeaving : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourGroupLeaving()
        {
            var data = DataGroup.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Touched = data.Touched;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active./// </summary>
        private HashSet<int> Active { get; }

        /// <summary>Cached IDs considered touched./// </summary>
        private HashSet<int> Touched { get; set; }

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

            var tick = PatchAchievementManager.GetTick();
            if (!this.IsPlayerOnBlock)
            {
                if (this.Touched.Count == 0)
                {
                    return true;
                }

                foreach (var groupId in this.Touched)
                {
                    if (!this.Groups.TryGetValue(groupId, out var group))
                    {
                        continue;
                    }

                    group.ActivatedTick = tick;
                    lock (this.Active)
                    {
                        _ = this.Active.Add(groupId);
                    }
                }

                this.Touched.Clear();
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockGroupA)
                       || type == typeof(BlockGroupB)
                       || type == typeof(BlockGroupC)
                       || type == typeof(BlockGroupD)
                       || type == typeof(BlockGroupIceA)
                       || type == typeof(BlockGroupIceB)
                       || type == typeof(BlockGroupIceC)
                       || type == typeof(BlockGroupIceD)
                       || type == typeof(BlockGroupSnowA)
                       || type == typeof(BlockGroupSnowB)
                       || type == typeof(BlockGroupSnowC)
                       || type == typeof(BlockGroupSnowD);
            });
            var currentlyTouched = new HashSet<int>();
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                var groupId = block.GroupId;
                if (!this.Groups.TryGetValue(groupId, out var group))
                {
                    continue;
                }

                if (!group.State
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsGroup.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                _ = currentlyTouched.Add(groupId);
            }

            foreach (var groupId in this.Touched.Except(currentlyTouched))
            {
                if (!this.Groups.TryGetValue(groupId, out var group))
                {
                    continue;
                }

                group.ActivatedTick = tick;
                _ = this.Active.Add(groupId);
            }

            this.Touched = currentlyTouched;

            return true;
        }
    }
}
