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
    ///     Behaviour attached to the <see cref="BlockBasicSingleUse" />.
    /// </summary>
    public class BehaviourBasicSingleUse : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourBasicSingleUse(Direction leverDirections)
        {
            this.Data = DataBasic.Instance;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Countdown data.</summary>
        private DataBasic Data { get; }

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

            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockBasicSingleUse>();
            var collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockBasicSingleUseOn>();
            var collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockBasicSingleUseOff>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockBasicSingleUseSolid>();
            var collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockBasicSingleUseSolidOn>();
            var collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockBasicSingleUseSolidOff>();
            var collidingWithAnyLever = collidingWithLever || collidingWithLeverSolid;
            var collidingWithAnyLeverOn = collidingWithLeverOn || collidingWithLeverSolidOn;
            var collidingWithAnyLeverOff = collidingWithLeverOff || collidingWithLeverSolidOff;
            this.IsPlayerOnBlock = collidingWithAnyLever
                                   || collidingWithAnyLeverOn
                                   || collidingWithAnyLeverOff;

            if (!this.IsPlayerOnBlock)
            {
                this.Data.HasSwitched = false;
                return true;
            }

            if (this.Data.HasSwitched)
            {
                return true;
            }

            this.Data.HasSwitched = true;

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            IBlock block;
            if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
            {
                if (collidingWithLeverSolid)
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicSingleUseSolid>().First();
                }
                else if (collidingWithLeverSolidOn)
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicSingleUseSolidOn>().First();
                }
                else
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicSingleUseSolidOff>().First();
                }

                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }
            else
            {
                if (collidingWithLever)
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicSingleUse>().First();
                }
                else if (collidingWithLeverOn)
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicSingleUseOn>().First();
                }
                else
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicSingleUseOff>().First();
                }
            }

            var blockGroupId = (IBlockGroupId)block;
            if (this.Data.Touched.Contains(blockGroupId.GroupId))
            {
                return true;
            }

            _ = this.Data.Touched.Add(blockGroupId.GroupId);

            var stateBefore = this.Data.State;
            if (collidingWithAnyLever)
            {
                this.Data.State = !this.Data.State;
            }
            else if (collidingWithAnyLeverOn)
            {
                this.Data.State = true;
            }
            else if (collidingWithAnyLeverOff)
            {
                this.Data.State = false;
            }

            if (stateBefore == this.Data.State)
            {
                return true;
            }

            this.Data.Tick = PatchAchievementManager.GetTick();
            ModSounds.BasicFlip?.PlayOneShot();

            return true;
        }

        /// <summary>
        ///     Updates the directions a lever can be activated from the given directions.
        /// </summary>
        /// <param name="leverDirections">Directions a lever can be activated from.</param>
        public void UpdateDirections(Direction leverDirections) => this.LeverDirections = leverDirections;
    }
}
