namespace SwitchBlocks.Behaviours
{
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Settings;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockAutoReset" />.
    /// </summary>
    public class BehaviourAutoReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourAutoReset() => this.Data = DataAuto.Instance;

        /// <summary>Auto data.</summary>
        private DataAuto Data { get; }

        /// <inheritdoc />
        public float BlockPriority => ModConstants.PrioNormal;

        /// <inheritdoc />
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc />
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc />
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc />
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc />
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc />
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            var advCollisionInfo = behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo;
            if (advCollisionInfo is null)
            {
                return true;
            }

            var isReset = advCollisionInfo.IsCollidingWith<BlockAutoReset>();
            var isResetFull = advCollisionInfo.IsCollidingWith<BlockAutoResetFull>();
            this.IsPlayerOnBlock = isReset || isResetFull;

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            this.Data.WarnCount = 0;
            this.Data.ResetTick = PatchAchievementManager.GetTick();
            if (isReset && !this.Data.State)
            {
                this.Data.ResetTick += SettingsAuto.DurationOff;
            }

            return true;
        }
    }
}
