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
    using SwitchBlocks.Setups;
    using SwitchBlocks.Util;

    public class BehaviourSequencePlatform : IBlockBehaviour
    {
        private DataSequence Data { get; }
        public float BlockPriority => 2.0f;
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourSequencePlatform() => this.Data = DataSequence.Instance;

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
            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockSequenceA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowC>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceD>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceIceD>()
                || advCollisionInfo.IsCollidingWith<BlockSequenceSnowD>();

            if (!this.IsPlayerOnBlock)
            {
                if (SettingsSequence.DisableOnLeave
                    && SettingsSequence.Duration == 0
                    && this.Data.Touched > 0)
                {
                    this.Data.SetTick(this.Data.Touched, int.MinValue);
                    _ = this.Data.Active.Add(this.Data.Touched);
                }
                return true;
            }

            var blocks = advCollisionInfo.GetCollidedBlocks().Where(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockSequenceA)
                || type == typeof(BlockSequenceIceA)
                || type == typeof(BlockSequenceSnowA)
                || type == typeof(BlockSequenceB)
                || type == typeof(BlockSequenceIceB)
                || type == typeof(BlockSequenceSnowB)
                || type == typeof(BlockSequenceC)
                || type == typeof(BlockSequenceIceC)
                || type == typeof(BlockSequenceSnowC)
                || type == typeof(BlockSequenceD)
                || type == typeof(BlockSequenceIceD)
                || type == typeof(BlockSequenceSnowD);
            });
            foreach (var block in blocks.Cast<IBlockGroupId>())
            {
                var groupId = block.GroupId;
                if (!this.Data.GetState(groupId)
                    || this.Data.Touched != (groupId - 1)
                    || !Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsSequence.PlatformDirections,
                        (IBlock)block))
                {
                    continue;
                }

                if (SettingsSequence.Duration == 0)
                {
                    if (groupId > 1)
                    {
                        this.Data.SetTick(groupId - 1, int.MinValue);
                        _ = this.Data.Active.Add(groupId - 1);
                    }
                }
                else
                {
                    var tick = AchievementManager.GetTicks();
                    this.Data.SetTick(groupId, tick + SettingsSequence.Duration);
                    _ = this.Data.Active.Add(groupId);
                }
                if (groupId < SetupSequence.SequenceCount)
                {
                    this.Data.SetTick(groupId + 1, int.MaxValue);
                    _ = this.Data.Active.Add(groupId + 1);
                }
                this.Data.Touched = groupId;
                break;
            }

            return true;
        }
    }
}
