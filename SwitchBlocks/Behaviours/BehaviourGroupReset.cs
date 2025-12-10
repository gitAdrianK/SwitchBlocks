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
    ///     Behaviour attached to the <see cref="BlockGroupReset" />.
    /// </summary>
    public class BehaviourGroupReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourGroupReset(Direction leverDirections)
        {
            var data = DataGroup.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Finished = data.Finished;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active.</summary>
        private HashSet<int> Active { get; }

        /// <summary>Cached IDs considered finished.</summary>
        private HashSet<int> Finished { get; }

        /// <summary>Get or set the group data's HasSwitched.</summary>
        private static bool HasSwitched
        {
            get => DataGroup.Instance.HasSwitched;
            set => DataGroup.Instance.HasSwitched = value;
        }

        /// <summary>Lever directions.</summary>
        private Direction LeverDirections { get; }

        /// <summary>Get or set the group data's Touched.</summary>
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

            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupResetSolid>();
            this.IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
            if (!this.IsPlayerOnBlock)
            {
                HasSwitched = false;
                return true;
            }

            if (HasSwitched)
            {
                return true;
            }

            HasSwitched = true;

            IBlock block;
            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithResetSolid)
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockGroupResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }
            else
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockGroupReset>().First();
            }

            // If the only reset id is 0, reset all groups.
            var resetIds = ((IResetGroupIds)block).ResetIDs;
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
