namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Settings;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockSandLever" />.
    /// </summary>
    public class BehaviourSandLever : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSandLever() => this.Data = DataSand.Instance;

        /// <summary>Sand data.</summary>
        private DataSand Data { get; }

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

            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockSandLever>();
            var collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockSandLeverOn>();
            var collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockSandLeverOff>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockSandLeverSolid>();
            var collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOn>();
            var collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOff>();
            var collidingWithAnyLever = collidingWithLever || collidingWithLeverSolid;
            var collidingWithAnyLeverOn = collidingWithLeverOn || collidingWithLeverSolidOn;
            var collidingWithAnyLeverOff = collidingWithLeverOff || collidingWithLeverSolidOff;
            this.IsPlayerOnBlock = collidingWithAnyLever
                                   || collidingWithAnyLeverOn
                                   || collidingWithAnyLeverOff;

            if (this.IsPlayerOnBlock)
            {
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
                        block = advCollisionInfo.GetCollidedBlocks<BlockSandLeverSolid>().First();
                    }
                    else if (collidingWithLeverSolidOn)
                    {
                        block = advCollisionInfo.GetCollidedBlocks<BlockSandLeverSolidOn>().First();
                    }
                    else
                    {
                        block = advCollisionInfo.GetCollidedBlocks<BlockSandLeverSolidOff>().First();
                    }

                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                            SettingsSand.LeverDirections,
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

                if (stateBefore != this.Data.State)
                {
                    ModSounds.SandFlip?.PlayOneShot();
                }
            }
            else
            {
                this.Data.HasSwitched = false;
            }

            return true;
        }
    }
}
