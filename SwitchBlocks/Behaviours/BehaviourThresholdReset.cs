namespace SwitchBlocks.Behaviours
{
    using System;
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using JumpKing.SaveThread;
    using Patches;
    using Settings;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockThresholdReset" />.
    /// </summary>
    public class BehaviourThresholdReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourThresholdReset(SettingsThreshold settings)
        {
            this.Data = DataThreshold.Instance;
            this.Stat = settings.Stat;
            this.LeverDirections = settings.LeverDirections;
        }

        /// <summary>Threshold data.</summary>
        private DataThreshold Data { get; }

        /// <summary>Lever directions.</summary>
        private Direction LeverDirections { get; set; }

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

            var isOnBlock = advCollisionInfo.IsCollidingWith<BlockThresholdReset>();
            var isOnBlockSolid = advCollisionInfo.IsCollidingWith<BlockThresholdResetSolid>();
            this.IsPlayerOnBlock = isOnBlock || isOnBlockSolid;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (isOnBlockSolid)
            {
                var block = advCollisionInfo.GetCollidedBlocks<BlockThresholdResetSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
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
                case Stat.Victory:
                case Stat.Boots:
                case Stat.Ring:
                case Stat.BootsRing:
                    break;
                default:
                    throw new ArgumentOutOfRangeException("Unknown stat: " + this.Stat);
            }

            return true;
        }

        /// <summary>
        ///     Updates the settings for threshold.
        /// </summary>
        /// <param name="settings">New threshold settings.</param>
        public void UpdateSettings(SettingsThreshold settings)
        {
            this.Stat = settings.Stat;
            if (this.Stat == Stat.Victory)
            {
                this.Data.State = CompletedUGCSave.ContainsCompletion(Game1.instance.contentManager.level.ID);
            }

            this.LeverDirections = settings.LeverDirections;
        }
    }
}
