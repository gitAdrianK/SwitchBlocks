namespace SwitchBlocks.Behaviours
{
    using System;
    using Blocks;
    using Data;
    using Dummy;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockSandOff" />.
    /// </summary>
    internal class BehaviourSandOff : IBlockBehaviour
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="collisionQuery">
        ///     <see cref="ICollisionQuery" />
        /// </param>
        public BehaviourSandOff(ICollisionQuery collisionQuery)
        {
            this.CollisionQuery = collisionQuery;
            this.Data = DataSand.Instance;
        }

        /// <summary>Sand data.</summary>
        private DataSand Data { get; }

        /// <summary>Collision query.</summary>
        private ICollisionQuery CollisionQuery { get; }

        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioLate;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.IsCollidingWith<BlockSandOff>())
            {
                return !this.IsPlayerOnBlock;
            }

            return false;
        }

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (!info.IsCollidingWith<BlockSandOff>() || this.IsPlayerOnBlock)
            {
                return false;
            }

            if (this.Data.State)
            {
                return behaviourContext.BodyComp.Velocity.Y < 0.0f;
            }

            return behaviourContext.BodyComp.Velocity.Y >= 0.0f;
        }

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
            => this.IsPlayerOnBlock ? inputXVelocity * 0.25f : inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            var bodyComp = behaviourContext.BodyComp;
            var multiplier = this.IsPlayerOnBlock && bodyComp.Velocity.Y <= 0.0f ? 0.5f : 1.0f;
            var result = inputYVelocity * multiplier;
            if (!this.IsPlayerOnBlock && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0.0f)
            {
                bodyComp.Position.Y += 1.0f;
            }

            if (BehaviourPost.IsPlayerOnTypeSandUp)
            {
                result -= 0.75f;
            }

            return result;
        }

        /// <inheritdoc />
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
            _ = this.CollisionQuery.CheckCollision(hitbox, out _, out AdvCollisionInfo info);
            this.IsPlayerOnBlock = info.IsCollidingWith<BlockSandOff>();
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            BehaviourPost.IsPlayerOnTypeSand = true;
            if (!this.Data.State)
            {
                BehaviourPost.IsPlayerOnTypeSandUp = true;
            }

            PatchBodyComp.SetKnocked(bodyComp, false);
            Camera.UpdateCamera(hitbox.Center);
            bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
            return true;
        }
    }
}
