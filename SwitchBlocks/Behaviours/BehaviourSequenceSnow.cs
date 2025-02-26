namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    public class BehaviourSequenceSnow : IBlockBehaviour
    {
        private DataSequence Data { get; }
        public float BlockPriority => 2.0f;
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourSequenceSnow() => this.Data = DataSequence.Instance;

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
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowD>();

            if (!this.IsPlayerOnBlock || BehaviourPost.IsPlayerOnSnow)
            {
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockSequenceSnowA)
                || type == typeof(BlockSequenceSnowB)
                || type == typeof(BlockSequenceSnowC)
                || type == typeof(BlockSequenceSnowD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                if (this.Data.GetState(block.GroupId))
                {
                    BehaviourPost.IsPlayerOnSnow = true;
                    break;
                }
            }

            return true;
        }
    }
}
