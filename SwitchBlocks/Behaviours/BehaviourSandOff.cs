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

    internal class BehaviourSandOff : IBlockBehaviour
    {
        /// <summary>Sand data.</summary>
        private DataSand Data { get; }
        /// <summary>Collision query.</summary>
        private ICollisionQuery CollisionQuery { get; }
        /// <inheritdoc/>
        public float BlockPriority => ModConsts.PRIO_LATE;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourSandOff(ICollisionQuery collisionQuery)
        {
            this.CollisionQuery = collisionQuery;
            this.Data = DataSand.Instance;
        }

        /// <inheritdoc/>
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockSandOff>())
            {
                return !this.IsPlayerOnBlock;
            }
            return false;
        }

        /// <inheritdoc/>
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockSandOff>() && !this.IsPlayerOnBlock)
            {
                if (!this.Data.State)
                {
                    return behaviourContext.BodyComp.Velocity.Y >= 0.0f;
                }
                else
                {
                    return behaviourContext.BodyComp.Velocity.Y < 0.0f;
                }
            }
            return false;
        }

        /// <inheritdoc/>
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc/>
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
            => this.IsPlayerOnBlock ? inputXVelocity * 0.25f : inputXVelocity;

        /// <inheritdoc/>
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            // I don't know what all the stuff inside the vanilla behaviour is for
            // and I won't either.
            if (this.IsPlayerOnBlock
                && !this.Data.State
                && behaviourContext.BodyComp.Velocity.Y >= -0.75f)
            {
                return inputYVelocity - (2.0f * PlayerValues.GRAVITY);
            }
            return inputYVelocity;
        }

        /// <inheritdoc/>
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo == null)
            {
                return true;
            }

            var bodyComp = behaviourContext.BodyComp;
            var hitbox = bodyComp.GetHitbox();
            // Turns out doing it this way doesn't have the problem of the player bouncing on top.
            _ = this.CollisionQuery.CheckCollision(hitbox, out var _, out AdvCollisionInfo info);
            this.IsPlayerOnBlock = info.IsCollidingWith<BlockSandOff>();
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            BehaviourPost.IsPlayerOnSand |= true;

            if (!this.Data.State)
            {
                BehaviourPost.IsPlayerOnSandUp |= true;
                bodyComp.Velocity.Y = Math.Min(-0.75f, bodyComp.Velocity.Y);
            }
            else
            {
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
            }
            _ = Traverse.Create(bodyComp).Field("_knocked").SetValue(false);
            Camera.UpdateCamera(hitbox.Center);
            return true;
        }
    }
}
