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

    public class BehaviourGroupReset : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

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
            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupResetSolid>();
            this.IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
            if (!this.IsPlayerOnBlock)
            {
                DataGroup.HasSwitched = false;
                return true;
            }

            if (DataGroup.HasSwitched)
            {
                return true;
            }
            DataGroup.HasSwitched = true;

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
                _ = Parallel.ForEach(DataGroup.Active, group
                    => DataGroup.Groups[group].ActivatedTick = int.MaxValue);
                _ = Parallel.ForEach(DataGroup.Finished, group =>
                {
                    DataGroup.Groups[group].ActivatedTick = int.MaxValue;
                    _ = DataGroup.Active.Add(group);
                });
                DataGroup.Finished.Clear();
                DataGroup.Touched.Clear();
            }
            else
            {
                _ = Parallel.ForEach(block.ResetIds, id =>
                {
                    if (DataGroup.Groups.TryGetValue(id, out var blockGroup))
                    {
                        blockGroup.ActivatedTick = int.MaxValue;
                        _ = DataGroup.Active.Add(id);
                        _ = DataGroup.Finished.Remove(id);
                    }
                });
            }

            return true;
        }
    }
}
