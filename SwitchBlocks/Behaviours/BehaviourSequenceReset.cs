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
    ///     Behaviour attached to the <see cref="BlockSequenceReset" />.
    /// </summary>
    public class BehaviourSequenceReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSequenceReset(int[] defaultActive, Direction leverDirections)
        {
            var data = DataSequence.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Finished = data.Finished;
            this.DefaultActive = defaultActive;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active.</summary>
        private HashSet<int> Active { get; }

        /// <summary>Cached IDs considered finished.</summary>
        private HashSet<int> Finished { get; }

        /// <summary>Get or set the sequence data's HasSwitched.</summary>
        private static bool HasSwitched
        {
            get => DataSequence.Instance.HasSwitched;
            set => DataSequence.Instance.HasSwitched = value;
        }

        ///<summary>Default active.</summary>
        private int[] DefaultActive { get; }

        /// <summary>Lever directions.</summary>
        private Direction LeverDirections { get; }

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

            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockSequenceReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockSequenceResetSolid>();
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
                var solid = advCollisionInfo.GetCollidedBlocks<BlockSequenceResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        solid))
                {
                    return true;
                }

                block = solid;
            }
            else
            {
                block = advCollisionInfo
                    .GetCollidedBlocks<BlockSequenceReset>().First();
            }

            // If the only reset id is 0, reset to default.
            var resetIds = ((IResetGroupIds)block).ResetIDs;
            if (resetIds.Length == 1 && resetIds[0] == 0)
            {
                foreach (var groupId in this.Active)
                {
                    if (!this.Groups.TryGetValue(groupId, out var activeGroup))
                    {
                        continue;
                    }

                    activeGroup.ActivatedTick = 0;
                }

                foreach (var groupId in this.Finished)
                {
                    if (!this.Groups.TryGetValue(groupId, out var finishedGroup))
                    {
                        continue;
                    }

                    finishedGroup.ActivatedTick = 0;
                    _ = this.Active.Add(groupId);
                }

                this.Finished.Clear();

                foreach (var defaultId in this.DefaultActive)
                {
                    if (!this.Groups.TryGetValue(defaultId, out var group))
                    {
                        continue;
                    }

                    group.ActivatedTick = int.MaxValue;
                    _ = this.Active.Add(defaultId);
                }
            }
            else
            {
                var first = resetIds[0];
                if (this.Groups.TryGetValue(first, out var group))
                {
                    group.ActivatedTick = int.MaxValue;
                    _ = this.Active.Add(first);
                    _ = this.Finished.Remove(first);
                }

                foreach (var resetId in resetIds.Skip(1))
                {
                    if (!this.Groups.TryGetValue(resetId, out group))
                    {
                        continue;
                    }

                    group.ActivatedTick = 0;
                    _ = this.Active.Add(resetId);
                    _ = this.Finished.Remove(resetId);
                }
            }

            return true;
        }
    }
}
