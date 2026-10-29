namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using Blocks;
    using Data;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Settings;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockAutoReset" />.
    /// </summary>
    public class BehaviourAutoReset : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourAutoReset(SettingsAuto settings)
        {
            this.Data = DataAuto.Instance;
            this.DurationOff = settings.DurationOff;
            this.LeverDirections = settings.LeverDirections;
        }

        /// <summary>Auto data.</summary>
        private DataAuto Data { get; }

        /// <summary>Off duration.</summary>
        private int DurationOff { get; set; }

        /// <summary>Lever directions.</summary>
        public Direction LeverDirections { get; private set; }

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

            var isReset = advCollisionInfo.IsCollidingWith<BlockAutoReset>();
            var isResetFull = advCollisionInfo.IsCollidingWith<BlockAutoResetFull>();
            var isResetSolid = advCollisionInfo.IsCollidingWith<BlockAutoResetSolid>();
            var isResetFullSolid = advCollisionInfo.IsCollidingWith<BlockAutoResetFullSolid>();
            this.IsPlayerOnBlock = isReset || isResetFull || isResetSolid || isResetFullSolid;

            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (isResetSolid || isResetFullSolid)
            {
                var block = isResetSolid
                    ? advCollisionInfo.GetCollidedBlocks<BlockAutoResetSolid>().First()
                    : advCollisionInfo.GetCollidedBlocks<BlockAutoResetFullSolid>().First();

                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        this.LeverDirections,
                        block))
                {
                    return true;
                }
            }

            this.Data.ResetTick = PatchAchievementManager.GetTick();
            if ((isReset || isResetSolid) && !this.Data.State)
            {
                this.Data.ResetTick += this.DurationOff;
            }

            return true;
        }

        /// <summary>
        ///     Updates the settings for auto.
        /// </summary>
        /// <param name="settings">New auto settings.</param>
        public void UpdateSettings(SettingsAuto settings)
        {
            this.DurationOff = settings.DurationOff;
            this.LeverDirections = settings.LeverDirections;
        }
    }
}
