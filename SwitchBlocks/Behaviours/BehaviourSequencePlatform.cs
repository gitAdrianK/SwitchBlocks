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
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }
        public static bool IsPlayerOnIce { get; set; }

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
                    && DataSequence.Touched > 0)
                {
                    DataSequence.SetTick(DataSequence.Touched, int.MinValue);
                    _ = DataSequence.Active.Add(DataSequence.Touched);
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
                if (!DataSequence.GetState(groupId)
                    || DataSequence.Touched != (groupId - 1)
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
                        DataSequence.SetTick(groupId - 1, int.MinValue);
                        _ = DataSequence.Active.Add(groupId - 1);
                    }
                }
                else
                {
                    var tick = AchievementManager.GetTicks();
                    DataSequence.SetTick(groupId, tick + SettingsSequence.Duration);
                    _ = DataSequence.Active.Add(groupId);
                }
                if (groupId < SetupSequence.SequenceCount)
                {
                    DataSequence.SetTick(groupId + 1, int.MaxValue);
                    _ = DataSequence.Active.Add(groupId + 1);
                }
                DataSequence.Touched = groupId;
                break;
            }

            return true;
        }
    }
}
