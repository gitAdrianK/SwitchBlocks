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

    public class BehaviourSequenceReset : IBlockBehaviour
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
            var collidingWithReset = advCollisionInfo.IsCollidingWith<BlockSequenceReset>();
            var collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockSequenceResetSolid>();
            this.IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
            if (!this.IsPlayerOnBlock)
            {
                DataSequence.HasSwitched = false;
                return true;
            }

            if (DataSequence.HasSwitched)
            {
                return true;
            }
            DataSequence.HasSwitched = true;

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

            _ = Parallel.ForEach(DataSequence.Active, group
                => DataSequence.Groups[group].ActivatedTick = int.MinValue);
            _ = Parallel.ForEach(DataSequence.Finished, group =>
            {
                DataSequence.Groups[group].ActivatedTick = int.MinValue;
                _ = DataSequence.Active.Add(group);
            });
            DataSequence.SetTick(1, int.MaxValue);
            DataSequence.Touched = 0;
            _ = DataSequence.Active.Add(1);
            DataSequence.Finished.Clear();

            return true;
        }
    }
}
