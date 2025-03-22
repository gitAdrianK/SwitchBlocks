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

    /// <summary>
    /// Behaviour attached to the countdown lever block.
    /// </summary>
    public class BehaviourCountdownLever : IBlockBehaviour
    {
        /// <summary>Countdown data.</summary>
        private DataCountdown Data { get; }
        /// <inheritdoc/>
        public float BlockPriority => 2.0f;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourCountdownLever() => this.Data = DataCountdown.Instance;

        /// <inheritdoc/>
        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc/>
        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        /// <inheritdoc/>
        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        /// <inheritdoc/>
        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        /// <inheritdoc/>
        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

        /// <inheritdoc/>
        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            var advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownLever>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockCountdownLeverSolid>();
            this.IsPlayerOnBlock = collidingWithLever || collidingWithLeverSolid;

            if (this.IsPlayerOnBlock)
            {
                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid)
                {
                    var block = advCollisionInfo.GetCollidedBlocks().First(b => b.GetType() == typeof(BlockCountdownLeverSolid));
                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsCountdown.LeverDirections,
                        block))
                    {
                        return true;
                    }
                }

                this.Data.ActivatedTick = AchievementManager.GetTick();

                if (this.Data.HasSwitched)
                {
                    return true;
                }

                if (!this.Data.State)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }

                this.Data.HasSwitched = true;
                this.Data.State = true;
            }
            else
            {
                this.Data.HasSwitched = false;
            }
            return true;
        }
    }
}
