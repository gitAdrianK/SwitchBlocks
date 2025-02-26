namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    public class BehaviourGroupDuration : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        private DataGroup Data { get; }
        public bool IsPlayerOnBlock { get; set; }
        public static bool IsPlayerOnIce { get; set; }

        public BehaviourGroupDuration() => this.Data = DataGroup.Instance;

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
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowC>()
                || advCollisionInfo.IsCollidingWith<BlockGroupD>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceD>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowD>();

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            var tick = AchievementManager.GetTicks();
            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockGroupA)
                || type == typeof(BlockGroupIceA)
                || type == typeof(BlockGroupSnowA)
                || type == typeof(BlockGroupB)
                || type == typeof(BlockGroupIceB)
                || type == typeof(BlockGroupSnowB)
                || type == typeof(BlockGroupC)
                || type == typeof(BlockGroupIceC)
                || type == typeof(BlockGroupSnowC)
                || type == typeof(BlockGroupD)
                || type == typeof(BlockGroupIceD)
                || type == typeof(BlockGroupSnowD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                var groupId = block.GroupId;
                if (!this.Data.GetState(groupId)
                    || this.Data.Touched.Contains(groupId)
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                    SettingsGroup.PlatformDirections,
                    (IBlock)block))
                {
                    continue;
                }
                this.Data.SetTick(groupId, tick + SettingsGroup.Duration);
                _ = this.Data.Active.Add(groupId);
                _ = this.Data.Touched.Add(groupId);
            }

            return true;
        }
    }
}
