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
    ///     Behaviour attached to the <see cref="BlockSandOn" />.
    /// </summary>
    public class BehaviourSandOn : IBlockBehaviour
    {
        /// <summary>
        ///     Ctor.
        /// </summary>
        /// <param name="collisionQuery">
        ///     <see cref="ICollisionQuery" />
        /// </param>
        public BehaviourSandOn(ICollisionQuery collisionQuery)
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
            if (info.IsCollidingWith<BlockSandOn>())
            {
                return !this.IsPlayerOnBlock;
            }

            return false;
        }

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (!info.IsCollidingWith<BlockSandOn>() || this.IsPlayerOnBlock)
            {
                return false;
            }

            if (this.Data.State)
            {
                return behaviourContext.BodyComp.Velocity.Y >= 0.0f;
            }

            return behaviourContext.BodyComp.Velocity.Y < 0.0f;
        }

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
            => this.IsPlayerOnBlock ? inputXVelocity * 0.25f : inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            // I don't know what all the stuff inside the vanilla behaviour is for
            // and I won't either.
            if (this.IsPlayerOnBlock
                && this.Data.State
                && behaviourContext.BodyComp.Velocity.Y >= -0.75f)
            {
                return inputYVelocity - (2.0f * PlayerValues.GRAVITY);
            }

            return inputYVelocity;
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
            this.IsPlayerOnBlock = info.IsCollidingWith<BlockSandOn>();
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            BehaviourPost.IsPlayerOnSand = true;

            if (this.Data.State)
            {
                BehaviourPost.IsPlayerOnSandUp = true;
                bodyComp.Velocity.Y = Math.Min(-0.75f, bodyComp.Velocity.Y);
            }
            else
            {
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
            }

            PatchBodyComp.SetKnocked(false);
            Camera.UpdateCamera(hitbox.Center);
            return true;
        }
    }
}
