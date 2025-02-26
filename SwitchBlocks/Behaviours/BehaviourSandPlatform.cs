namespace SwitchBlocks.Behaviours
{
    using System;
    using HarmonyLib;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;

    /// <summary>
    /// Behaviour related to sand platforms.
    /// </summary>
    public class BehaviourSandPlatform : IBlockBehaviour
    {
        public float BlockPriority => 1.0f;

        private DataSand Data { get; }
        public bool IsPlayerOnBlock { get; set; }
        public bool IsPlayerOnBlockOn { get; set; }
        public bool IsPlayerOnBlockOff { get; set; }

        public BehaviourSandPlatform() => this.Data = DataSand.Instance;


        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            // 0.25f from SandBlockBehaviour results in the wrong X speed, 0.5f seems to be about right.
            var num = this.Data.HasEntered ? 0.5f : 1.0f;
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {

            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return inputYVelocity;
            }
            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            if (!(advCollisionInfo.IsCollidingWith<BlockSandOn>() || advCollisionInfo.IsCollidingWith<BlockSandOff>()))
            {
                return inputYVelocity;
            }

            var bodyComp = behaviourContext.BodyComp;
            var num = (this.Data.HasEntered && bodyComp.Velocity.Y <= 0.0f) ? 0.75f : 1.0f;
            var result = inputYVelocity * num;
            if (!this.Data.HasEntered && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0.0f)
            {
                bodyComp.Position.Y += 1.0f;
            }
            return result;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

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

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if ((info.IsCollidingWith<BlockSandOn>() && this.Data.State) || (info.IsCollidingWith<BlockSandOff>() && !this.Data.State))
            {
                if (this.Data.HasEntered)
                {
                    return false;
                }
                this.Data.HasEntered = behaviourContext.BodyComp.Velocity.Y < 0.0f;
                return !this.Data.HasEntered;
            }
            if ((info.IsCollidingWith<BlockSandOn>() && !this.Data.State) || (info.IsCollidingWith<BlockSandOff>() && this.Data.State))
            {
                if (this.Data.HasEntered)
                {
                    return false;
                }
                this.Data.HasEntered = behaviourContext.BodyComp.Velocity.Y >= 0.0f;
                return !this.Data.HasEntered;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            var bodyComp = behaviourContext.BodyComp;

            this.IsPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockSandOn>();
            this.IsPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockSandOff>();
            this.IsPlayerOnBlock = this.IsPlayerOnBlockOn || this.IsPlayerOnBlockOff;

            if (!this.IsPlayerOnBlock)
            {
                this.Data.HasEntered = false;
            }

            if (this.Data.HasEntered)
            {
                if ((this.IsPlayerOnBlockOn && this.Data.State) || (this.IsPlayerOnBlockOff && !this.Data.State))
                {
                    bodyComp.Position.Y -= 0.75f;
                }
                _ = Traverse.Create(bodyComp).Field("_knocked").SetValue(false);
                Camera.UpdateCamera(bodyComp.GetHitbox().Center);
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
            }
            return true;
        }
    }
}
