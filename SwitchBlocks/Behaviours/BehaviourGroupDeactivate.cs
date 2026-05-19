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
    ///     Behaviour attached to the <see cref="BlockGroupDeactivate" />.
    /// </summary>
    public class BehaviourGroupDeactivate : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourGroupDeactivate(Direction leverDirections)
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
        private Direction LeverDirections { get; set; }

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
            if (advCollisionInfo == null)
            {
                return true;
            }

            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupDeactivate>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupDeactivateSolid>();
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
                block = advCollisionInfo.GetCollidedBlocks<BlockGroupDeactivateSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }
            else
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockGroupDeactivate>().First();
            }

            // If the only deactivate id is 0, deactivate all groups.
            var deactivateIds = ((IMultipleGroupIds)block).Ids;
            if (deactivateIds.Length == 1 && deactivateIds[0] == 0)
            {
                foreach (var keyValuePair in
                         this.Groups.Where(keyValuePair => !this.Finished.Contains(keyValuePair.Key)))
                {
                    keyValuePair.Value.ActivatedTick = int.MinValue;
                    _ = this.Active.Add(keyValuePair.Key);
                }
            }
            else
            {
                foreach (var deactivateId in deactivateIds)
                {
                    if (this.Finished.Contains(deactivateId)
                        || !this.Groups.TryGetValue(deactivateId, out var group))
                    {
                        continue;
                    }

                    group.ActivatedTick = int.MinValue;
                    _ = this.Active.Add(deactivateId);
                }
            }

            return true;
        }

        /// <summary>
        ///     Updates the directions a lever can be activated from the given directions.
        /// </summary>
        /// <param name="leverDirections">Directions a lever can be activated from.</param>
        public void UpdateDirections(Direction leverDirections) => this.LeverDirections = leverDirections;
    }
}
