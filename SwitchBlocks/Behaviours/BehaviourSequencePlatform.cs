namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Setups;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour attached to the sequence A block.
    /// </summary>
    public class BehaviourSequencePlatform : IBlockBehaviour
    {
        /// <summary>Cached mappings of <see cref="BlockGroup"/>s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; set; }
        /// <summary>Cached ids considered active./// </summary>
        private HashSet<int> Active { get; set; }
        /// <summary>Id considered touched./// </summary>
        private int Touched
        {
            get => DataSequence.Instance.Touched;
            set => DataSequence.Instance.Touched = value;
        }
        /// <inheritdoc/>
        public float BlockPriority => ModConsts.PRIO_NORMAL;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourSequencePlatform()
        {
            var data = DataSequence.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
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
                var touched = this.Touched;
                if (SettingsSequence.DisableOnLeave
                    && SettingsSequence.Duration == 0
                    && touched > 0)
                {
                    if (!this.Groups.TryGetValue(touched, out var group))
                    {
                        return true;
                    }
                    group.ActivatedTick = int.MinValue;
                    _ = this.Active.Add(touched);
                }
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockSequenceA)
                || type == typeof(BlockSequenceB)
                || type == typeof(BlockSequenceC)
                || type == typeof(BlockSequenceD)
                || type == typeof(BlockSequenceIceA)
                || type == typeof(BlockSequenceIceB)
                || type == typeof(BlockSequenceIceC)
                || type == typeof(BlockSequenceIceD)
                || type == typeof(BlockSequenceSnowA)
                || type == typeof(BlockSequenceSnowB)
                || type == typeof(BlockSequenceSnowC)
                || type == typeof(BlockSequenceSnowD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                var groupId = block.GroupId;
                if (!this.Groups.TryGetValue(groupId, out var group))
                {
                    continue;
                }
                if (!group.State
                    || this.Touched != (groupId - 1)
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsSequence.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                if (SettingsSequence.Duration == 0)
                {
                    if (groupId > 1)
                    {
                        if (!this.Groups.TryGetValue(groupId - 1, out var prevGroup))
                        {
                            continue;
                        }
                        prevGroup.ActivatedTick = int.MinValue;
                        _ = this.Active.Add(groupId - 1);
                    }
                }
                else
                {
                    var tick = AchievementManager.GetTick();
                    group.ActivatedTick = tick + SettingsSequence.Duration;
                    _ = this.Active.Add(groupId);
                }

                if (groupId < SetupSequence.SequenceCount)
                {
                    if (!this.Groups.TryGetValue(groupId + 1, out var nextGroup))
                    {
                        continue;
                    }
                    nextGroup.ActivatedTick = int.MaxValue;
                    _ = this.Active.Add(groupId + 1);
                }
                this.Touched = groupId;
                break;
            }

            return true;
        }
    }
}
