namespace SwitchBlocks.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Patching;
    using SwitchBlocks.Settings;

    /// <summary>
    /// Behaviour related to auto reset blocks.
    /// </summary>
    public class BehaviourAutoReset : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        private DataAuto Data { get; }
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourAutoReset() => this.Data = DataAuto.Instance;

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
            var isReset = advCollisionInfo.IsCollidingWith<BlockAutoReset>();
            var isResetFull = advCollisionInfo.IsCollidingWith<BlockAutoResetFull>();
            this.IsPlayerOnBlock = isReset || isResetFull;

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            this.Data.WarnCount = 0;
            this.Data.ResetTick = AchievementManager.GetTicks();
            if (isReset && !this.Data.State)
            {
                this.Data.ResetTick += SettingsAuto.DurationOff;
            }

            return true;
        }
    }
}
