namespace SwitchBlocks.Behaviours
{
    using System.Collections.Generic;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Settings;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockSequenceReset" />.
    /// </summary>
    public class BehaviourSequenceReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSequenceReset()
        {
            var data = DataSequence.Instance;
            this.Groups = data.Groups;
            this.Active = data.Active;
            this.Finished = data.Finished;
        }

        /// <summary>Cached mappings of <see cref="BlockGroup" />s to their id.</summary>
        private Dictionary<int, BlockGroup> Groups { get; }

        /// <summary>Cached IDs considered active.</summary>
        private HashSet<int> Active { get; }

        /// <summary>Cached IDs considered finished.</summary>
        private HashSet<int> Finished { get; }

        /// <summary>Get or set the sequence data's Touched.</summary>
        private static int Touched
        {
            set => DataSequence.Instance.Touched = value;
        }

        /// <summary>Get or set the sequence data's HasSwitched.</summary>
        private static bool HasSwitched
        {
            get => DataSequence.Instance.HasSwitched;
            set => DataSequence.Instance.HasSwitched = value;
        }

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

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithResetSolid)
            {
                var block = advCollisionInfo.GetCollidedBlocks<BlockSequenceResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsSequence.LeverDirections,
                        block))
                {
                    return true;
                }
            }

            foreach (var groupId in this.Active)
            {
                if (!this.Groups.TryGetValue(groupId, out var activeGroup))
                {
                    continue;
                }

                activeGroup.ActivatedTick = int.MinValue;
            }

            foreach (var groupId in this.Finished)
            {
                if (!this.Groups.TryGetValue(groupId, out var finishedGroup))
                {
                    continue;
                }

                finishedGroup.ActivatedTick = int.MinValue;
                _ = this.Active.Add(groupId);
            }

            this.Finished.Clear();

            if (this.Groups.TryGetValue(1, out var group))
            {
                group.ActivatedTick = int.MaxValue;
                _ = this.Active.Add(1);
            }

            Touched = 0;

            return true;
        }
    }
}
