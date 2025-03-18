namespace SwitchBlocks.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;

    public class BehaviourBasicOff : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        private DataBasic Data { get; }
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourBasicOff() => this.Data = DataBasic.Instance;

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
            var isOnBasic = advCollisionInfo.IsCollidingWith<BlockBasicOff>();
            var isOnIce = advCollisionInfo.IsCollidingWith<BlockBasicIceOff>();
            var isOnSnow = advCollisionInfo.IsCollidingWith<BlockBasicSnowOff>();
            this.IsPlayerOnBlock = !this.Data.State && (isOnBasic || isOnIce || isOnSnow);
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            if (!this.Data.State)
            {
                BehaviourPost.IsPlayerOnIce |= isOnIce;
                BehaviourPost.IsPlayerOnSnow |= isOnSnow;
            }

            return true;
        }
    }
}
