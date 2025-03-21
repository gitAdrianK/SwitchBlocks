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
        private DataSequence Data { get; }
        public float BlockPriority => 2.0f;
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourSequenceReset() => this.Data = DataSequence.Instance;

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
