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
    ///     Behaviour attached to the <see cref="BlockCountdownSingleUse" />.
    /// </summary>
    public class BehaviourCountdownSingleUse : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourCountdownSingleUse() => this.Data = DataCountdown.Instance;

        /// <summary>Countdown data.</summary>
        private DataCountdown Data { get; }

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

            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownSingleUse>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockCountdownSingleUseSolid>();
            this.IsPlayerOnBlock = collidingWithLever || collidingWithLeverSolid;
            if (!this.IsPlayerOnBlock)
            {
                return true;
            }

            IBlock block;
            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithLeverSolid)
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockCountdownSingleUseSolid>().First();
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsCountdown.LeverDirections,
                        block))
                {
                    return true;
                }
            }
            else
            {
                block = advCollisionInfo.GetCollidedBlocks<BlockCountdownSingleUse>().First();
            }

            var blockGroupId = (IBlockGroupId)block;
            if (this.Data.Touched.Contains(blockGroupId.GroupId))
            {
                return true;
            }

            this.Data.ActivatedTick = PatchAchievementManager.GetTick();
            _ = this.Data.Touched.Add(blockGroupId.GroupId);

            if (!this.Data.State)
            {
                ModSounds.CountdownFlip?.PlayOneShot();
            }

            this.Data.State = true;
            return true;
        }
    }
}
