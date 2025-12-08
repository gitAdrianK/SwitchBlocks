namespace SwitchBlocks.Behaviours
{
    using System;
    using Blocks;
    using Data;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;

    /// <summary>
    ///     Behaviour attached to both sand platform blocks.
    ///     This is technically a mistake, however it will be kept because GotIB already uses it.
    /// </summary>
    public class BehaviourSandLegacy : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourSandLegacy() => this.Data = DataSand.Instance;

        /// <summary>Sand data.</summary>
        private DataSand Data { get; }

        private bool IsPlayerOnBlockOn { get; set; }
        private bool IsPlayerOnBlockOff { get; set; }

        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioLate;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            // 0.25f from SandBlockBehaviour results in the wrong X speed, 0.5f seems to be about right.
            var num = this.Data.HasEntered ? 0.5f : 1.0f;
            return inputXVelocity * num;
        }

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo is null)
            {
                return inputYVelocity;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            if (!(advCollisionInfo.IsCollidingWith<BlockSandOn>() || advCollisionInfo.IsCollidingWith<BlockSandOff>()))
            {
                return inputYVelocity;
            }

            var bodyComp = behaviourContext.BodyComp;
            var num = this.Data.HasEntered && bodyComp.Velocity.Y <= 0.0f ? 0.75f : 1.0f;
            var result = inputYVelocity * num;
            if (!this.Data.HasEntered && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0.0f)
            {
                bodyComp.Position.Y += 1.0f;
            }

            return result;
        }

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (this.Data.HasEntered)
            {
                return false;
            }

            if (info.IsCollidingWith<BlockSandOn>() || info.IsCollidingWith<BlockSandOff>())
            {
                return !this.IsPlayerOnBlock;
            }

            return false;
        }

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if ((info.IsCollidingWith<BlockSandOn>() && this.Data.State) ||
                (info.IsCollidingWith<BlockSandOff>() && !this.Data.State))
            {
                if (this.Data.HasEntered)
                {
                    return false;
                }

                this.Data.HasEntered = behaviourContext.BodyComp.Velocity.Y < 0.0f;
                return !this.Data.HasEntered;
            }

            if ((!info.IsCollidingWith<BlockSandOn>() || this.Data.State) &&
                (!info.IsCollidingWith<BlockSandOff>() || !this.Data.State))
            {
                return false;
            }

            if (this.Data.HasEntered)
            {
                return false;
            }

            this.Data.HasEntered = behaviourContext.BodyComp.Velocity.Y >= 0.0f;
            return !this.Data.HasEntered;
        }

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo is null)
            {
                return true;
            }

            var bodyComp = behaviourContext.BodyComp;

            this.IsPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockSandOn>();
            this.IsPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockSandOff>();
            this.IsPlayerOnBlock = this.IsPlayerOnBlockOn || this.IsPlayerOnBlockOff;

            if (!this.IsPlayerOnBlock)
            {
                this.Data.HasEntered = false;
            }

            if (!this.Data.HasEntered)
            {
                return true;
            }

            if ((this.IsPlayerOnBlockOn && this.Data.State) || (this.IsPlayerOnBlockOff && !this.Data.State))
            {
                bodyComp.Position.Y -= 0.75f;
            }

            PatchBodyComp.SetKnocked(bodyComp, false);
            Camera.UpdateCamera(bodyComp.GetHitbox().Center);
            bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);

            return true;
        }
    }
}
