namespace SwitchBlocks.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    public class BehaviourJumpOn : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        private DataJump Data { get; }
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourJumpOn() => this.Data = DataJump.Instance;

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
            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockJumpOn>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockJumpIceOn>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockJumpSnowOn>();
            this.IsPlayerOnBlock = isOnBasic || isOnIce || isOnSnow;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (this.Data.State)
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
                    this.Data.SwitchOnceSafe = false;
                }
                if (this.Data.CanSwitchSafely)
                {
                    this.Data.CanSwitchSafely = !Intersecting.IsIntersectingBlocks(
                        behaviourContext,
                        typeof(BlockJumpOn),
                        typeof(BlockJumpIceOn),
                        typeof(BlockJumpSnowOn));
                }
            }

            return true;
        }
    }
}
