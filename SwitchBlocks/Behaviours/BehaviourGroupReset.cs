namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour attached to the group reset block.
    /// </summary>
    public class BehaviourGroupReset : IBlockBehaviour
    {
        /// <summary>Cached mappings of <see cref="BlockGroup"/>s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; set; }
        /// <summary>Cached ids considered active.</summary>
        private HashSet<int> Active { get; set; }
        /// <summary>Cached ids considered finished.</summary>
        private HashSet<int> Finished { get; set; }
        /// <summary>Get or set the sequence datas HasSwitched.</summary>
        private bool HasSwitched
        {
            get => DataGroup.Instance.HasSwitched;
            set => DataGroup.Instance.HasSwitched = value;
        }
        /// <summary>Get or set the sequence datas Touched.</summary>
        /// <inheritdoc/>
        public float BlockPriority => ModConsts.PRIO_NORMAL;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourGroupReset()
        {
            var data = DataGroup.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Finished = data.Finished;
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

            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupResetSolid>();
            this.IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
            if (!this.IsPlayerOnBlock)
            {
                this.HasSwitched = false;
                return true;
            }
            if (this.HasSwitched)
            {
                return true;
            }
            this.HasSwitched = true;

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithResetSolid)
            {
                var solid = advCollisionInfo.GetCollidedBlocks<BlockGroupResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                    SettingsGroup.LeverDirections,
                    solid))
                {
                    return true;
                }
            }

            var block = (IResetGroupIds)advCollisionInfo.GetCollidedBlocks().First(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockGroupReset)
                || type == typeof(BlockGroupResetSolid);
            });

            // If the only reset id is 0, reset all groups.
            var resetIds = block.ResetIds;
            if (resetIds.Length == 1 && resetIds[0] == 0)
            {
                foreach (var groupId in this.Active)
                {
                    if (!this.Groups.TryGetValue(groupId, out var group))
                    {
                        continue;
                    }
                    group.ActivatedTick = int.MaxValue;
                }
                foreach (var groupId in this.Finished)
                {
                    if (!this.Groups.TryGetValue(groupId, out var group))
                    {
                        continue;
                    }
                    group.ActivatedTick = int.MaxValue;
                    _ = this.Active.Add(groupId);
                }
                this.Finished.Clear();
            }
            else
            {
                foreach (var resetId in resetIds)
                {
                    if (!this.Groups.TryGetValue(resetId, out var group))
                    {
                        continue;
                    }
                    group.ActivatedTick = int.MaxValue;
                    _ = this.Active.Add(resetId);
                    _ = this.Finished.Remove(resetId);
                }
            }

            return true;
        }
    }
}
