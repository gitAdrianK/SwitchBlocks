namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using System.Threading.Tasks;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour attached to the sequence reset block.
    /// </summary>
    public class BehaviourSequenceReset : IBlockBehaviour
    {
        /// <summary>Sequence data.</summary>
        private DataSequence Data { get; }
        /// <inheritdoc/>
        public float BlockPriority => 2.0f;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourSequenceReset() => this.Data = DataSequence.Instance;

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
            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockSequenceReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockSequenceResetSolid>();
            this.IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
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

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithResetSolid)
            {
                var block = advCollisionInfo.GetCollidedBlocks().First(b => b.GetType() == typeof(BlockSequenceResetSolid));
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                    SettingsSequence.LeverDirections,
                    block))
                {
                    return true;
                }
            }

            _ = Parallel.ForEach(this.Data.Active, group
                => this.Data.Groups[group].ActivatedTick = int.MinValue);
            _ = Parallel.ForEach(this.Data.Finished, group =>
            {
                this.Data.Groups[group].ActivatedTick = int.MinValue;
                _ = this.Data.Active.Add(group);
            });
            this.Data.SetTick(1, int.MaxValue);
            this.Data.Touched = 0;
            _ = this.Data.Active.Add(1);
            this.Data.Finished.Clear();

            return true;
        }
    }
}
