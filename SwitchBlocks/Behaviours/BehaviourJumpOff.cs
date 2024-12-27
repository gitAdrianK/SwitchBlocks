namespace SwitchBlocks.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    public class BehaviourJumpOff : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockJumpOff>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockJumpIceOff>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockJumpSnowOff>();
            this.IsPlayerOnBlock = isOnBasic || isOnIce || isOnSnow;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (!DataJump.State)
            {
                if (isOnIce)
                {
                    BehaviourPost.IsPlayerOnIce = true;
                }

                if (isOnSnow)
                {
                    BehaviourPost.IsPlayerOnSnow = true;
                }
            }
            else
            {
                if (behaviourContext.BodyComp.IsOnGround)
                {
                    DataJump.SwitchOnceSafe = false;
                }
                if (DataJump.CanSwitchSafely)
                {
                    DataJump.CanSwitchSafely = !Intersecting.IsIntersectingBlocks(
                        behaviourContext,
                        typeof(BlockJumpOff),
                        typeof(BlockJumpIceOff),
                        typeof(BlockJumpSnowOff));
                }
            }

            return true;
        }
    }
}
