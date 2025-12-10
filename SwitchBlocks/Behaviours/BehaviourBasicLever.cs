namespace SwitchBlocks.Behaviours
{
    using System.Collections.Specialized;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockBasicLever" />.
    /// </summary>
    public class BehaviourBasicLever : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourBasicLever(BitVector32 leverDirections)
        {
            this.Data = DataBasic.Instance;
            this.LeverDirections = leverDirections;
        }

        /// <summary>Basic data.</summary>
        private DataBasic Data { get; }

        /// <summary>Lever directions.</summary>
        private BitVector32 LeverDirections { get; }

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

            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockBasicLever>();
            var collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverOn>();
            var collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverOff>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolid>();
            var collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOn>();
            var collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOff>();
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
            if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
            {
                IBlock block;
                if (collidingWithLeverSolid)
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicLeverSolid>().First();
                }
                else if (collidingWithLeverSolidOn)
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicLeverSolidOn>().First();
                }
                else
                {
                    block = advCollisionInfo.GetCollidedBlocks<BlockBasicLeverSolidOff>().First();
                }

                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }

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
    }
}
