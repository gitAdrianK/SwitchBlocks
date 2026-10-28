namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockJumpReset" />.
    /// </summary>
    public class BehaviourJumpReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourJumpReset(Direction leverDirections)
        {
            this.Data = DataJump.Instance;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Auto data.</summary>
        private DataJump Data { get; }

        /// <summary>Lever directions.</summary>
        private Direction LeverDirections { get; set; }

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

            var isOnBlock = advCollisionInfo.IsCollidingWith<BlockJumpReset>();
            var isOnBlockSolid = advCollisionInfo.IsCollidingWith<BlockJumpResetSolid>();
            this.IsPlayerOnBlock = isOnBlock || isOnBlockSolid;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (isOnBlockSolid)
            {
                var block = advCollisionInfo.GetCollidedBlocks<BlockJumpResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }

            if (this.Data.State)
            {
                this.Data.SwitchOnceSafe = true;
            }

            this.Data.Tick = PatchAchievementManager.GetTick();

            return true;
        }

        /// <summary>
        ///     Updates the directions a reset can be activated from the given directions.
        /// </summary>
        /// <param name="leverDirections">Directions a reset can be activated from.</param>
        public void UpdateDirections(Direction leverDirections) => this.LeverDirections = leverDirections;
    }
}
