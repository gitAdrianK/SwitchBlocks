namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using Blocks;
    using Data;
    using Entities;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using Patches;
    using Util;

    /// <summary>
    ///     Behaviour attached to the <see cref="BlockAutoChangeDuration" />.
    /// </summary>
    public class BehaviourAutoChangeDuration : IBlockBehaviour
    {
        /// <summary>Ctor.</summary>
        public BehaviourAutoChangeDuration(EntityLogicAuto entityLogic)
        {
            this.Data = DataAuto.Instance;
            this.EntityLogic = entityLogic;
        }

        /// <summary>Auto data.</summary>
        private DataAuto Data { get; }

        /// <summary>Logic entity.</summary>
        private EntityLogicAuto EntityLogic { get; }

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

            var change = advCollisionInfo.IsCollidingWith<BlockAutoChangeDuration>();
            var changeOn = advCollisionInfo.IsCollidingWith<BlockAutoChangeDurationOn>();
            var changeOff = advCollisionInfo.IsCollidingWith<BlockAutoChangeDurationOff>();
            this.IsPlayerOnBlock = change || changeOn || changeOff;

            if (!this.IsPlayerOnBlock)
            {
                this.Data.HasSwitched = false;
                return true;
            }

            if (this.Data.HasSwitched)
            {
                return true;
            }

            this.Data.HasSwitched = true;

            if (change)
            {
                var block = (IBlockDuration)advCollisionInfo.GetCollidedBlocks<BlockAutoChangeDuration>().First();
                var duration = block.Duration;
                if (duration == this.EntityLogic.DurationOn && duration == this.EntityLogic.DurationOff)
                {
                    return true;
                }

                this.EntityLogic.UpdateDurations(duration, duration);
            }
            else if (changeOn)
            {
                var block = (IBlockDuration)advCollisionInfo.GetCollidedBlocks<BlockAutoChangeDurationOn>().First();
                var duration = block.Duration;
                if (duration == this.EntityLogic.DurationOn)
                {
                    return true;
                }

                this.EntityLogic.UpdateDurations(duration, this.EntityLogic.DurationOff);
            }
            else
            {
                var block = (IBlockDuration)advCollisionInfo.GetCollidedBlocks<BlockAutoChangeDurationOff>().First();
                var duration = block.Duration;
                if (duration == this.EntityLogic.DurationOff)
                {
                    return true;
                }

                this.EntityLogic.UpdateDurations(this.EntityLogic.DurationOn, duration);
            }

            this.Data.ResetTick = PatchAchievementManager.GetTick();
            if (!this.Data.State)
            {
                this.Data.ResetTick += this.EntityLogic.DurationOff;
            }

            return true;
        }
    }
}
