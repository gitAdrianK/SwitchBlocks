namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patches;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour attached to the group A block should a duration be set.
    /// </summary>
    public class BehaviourGroupDuration : IBlockBehaviour
    {
        /// <summary>Cached mappings of <see cref="BlockGroup"/>s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; set; }
        /// <summary>Cached ids considered active./// </summary>
        private HashSet<int> Active { get; set; }
        /// <summary>Cached ids considered touched./// </summary>
        private HashSet<int> Touched { get; set; }
        /// <inheritdoc/>
        public float BlockPriority => ModConsts.PRIO_NORMAL;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourGroupDuration()
        {
            var data = DataGroup.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Touched = data.Touched;
        }

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

            foreach (var block in blocks.Cast<IBlockGroupId>())
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
                        SettingsGroup.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }
                group.ActivatedTick = tick + SettingsGroup.Duration;
                _ = this.Active.Add(groupId);
                _ = this.Touched.Add(groupId);
            }

            return true;
        }
    }
}
