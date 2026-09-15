namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockBasicReset" />.
    /// </summary>
    public class BehaviourBasicReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourBasicReset(Direction leverDirections)
        {
            this.Data = DataBasic.Instance;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Basic data.</summary>
        private DataBasic Data { get; }

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

            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockBasicReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockBasicResetSolid>();
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
                block = advCollisionInfo.GetCollidedBlocks<BlockBasicResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }
            else
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockBasicReset>().First();
            }

            // If the only reset id is 0, reset all groups.
            var resetIds = ((IMultipleGroupIds)block).Ids;
            if (resetIds.Length == 1 && resetIds[0] == 0)
            {
                this.Data.Touched.Clear();
            }
            else
            {
                foreach (var resetId in resetIds)
                {
                    this.Data.Touched.Remove(resetId);
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
