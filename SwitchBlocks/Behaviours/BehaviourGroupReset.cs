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
    /// Behaviour attached to the group reset block.
    /// </summary>
    public class BehaviourGroupReset : IBlockBehaviour
    {
        /// <summary>Group data.</summary>
        private DataGroup Data { get; }
        /// <inheritdoc/>
        public float BlockPriority => 2.0f;
        /// <inheritdoc/>
        public bool IsPlayerOnBlock { get; set; }

        /// <inheritdoc/>
        public BehaviourGroupReset() => this.Data = DataGroup.Instance;

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
            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupResetSolid>();
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
                var solid = advCollisionInfo.GetCollidedBlocks().First(b => b.GetType() == typeof(BlockGroupResetSolid));
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                    SettingsGroup.LeverDirections,
                    solid))
                {
                    return true;
                }
            }

            var block = (IResetGroupIds)advCollisionInfo.GetCollidedBlocks().First(b =>
            {
                var type = b.GetType();
                return type == typeof(BlockGroupReset)
                || type == typeof(BlockGroupResetSolid);
            });

            if (block.ResetIds.Length == 1 && block.ResetIds[0] == 0)
            {
                _ = Parallel.ForEach(this.Data.Active, group
                    => this.Data.Groups[group].ActivatedTick = int.MaxValue);
                _ = Parallel.ForEach(this.Data.Finished, group =>
                {
                    this.Data.Groups[group].ActivatedTick = int.MaxValue;
                    _ = this.Data.Active.Add(group);
                });
                this.Data.Finished.Clear();
                this.Data.Touched.Clear();
            }
            else
            {
                _ = Parallel.ForEach(block.ResetIds, id =>
                {
                    if (this.Data.Groups.TryGetValue(id, out var blockGroup))
                    {
                        blockGroup.ActivatedTick = int.MaxValue;
                        _ = this.Data.Active.Add(id);
                        _ = this.Data.Finished.Remove(id);
                    }
                });
            }

            return true;
        }
    }
}
