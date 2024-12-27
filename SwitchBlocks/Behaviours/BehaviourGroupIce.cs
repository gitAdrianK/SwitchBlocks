namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    public class BehaviourGroupIce : IBlockBehaviour
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
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupIceA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceD>();

            if (!this.IsPlayerOnBlock || BehaviourPost.IsPlayerOnIce)
            {
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockGroupIceA)
                || type == typeof(BlockGroupIceB)
                || type == typeof(BlockGroupIceC)
                || type == typeof(BlockGroupIceD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                if (DataGroup.GetState(block.GroupId))
                {
                    BehaviourPost.IsPlayerOnIce = true;
                    break;
                }
            }

            return true;
        }
    }
}
