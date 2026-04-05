namespace SwitchBlocks.Behaviours
{
    using System;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockThresholdReset" />.
    /// </summary>
    public class BehaviourThresholdReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourThresholdReset(Stat stat)
        {
            this.Data = DataThreshold.Instance;
            this.Stat = stat;
        }

        /// <summary>Threshold data.</summary>
        private DataThreshold Data { get; }

        /// <summary>The stat to look for.</summary>
        private Stat Stat { get; set; }

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
            if (advCollisionInfo == null)
            {
                return true;
            }

            this.IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockThresholdReset>();

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            this.Data.ResetTick = PatchAchievementManager.GetTick();
            switch (this.Stat)
            {
                case Stat.Jumps:
                    this.Data.ResetCount = PatchAchievementManager.GetJumps();
                    break;
                case Stat.Falls:
                    this.Data.ResetCount = PatchAchievementManager.GetFalls();
                    break;
                case Stat.Time:
                    this.Data.ResetCount = PatchAchievementManager.GetTick();
                    break;
                case Stat.Session:
                    this.Data.ResetCount = PatchAchievementManager.GetSession();
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown stat: " + this.Stat);
            }

            return true;
        }

        /// <summary>
        ///     Updates the stat to check for.
        /// </summary>
        /// <param name="stat">Stat to check for.</param>
        public void UpdateStat(Stat stat) => this.Stat = stat;
    }
}
