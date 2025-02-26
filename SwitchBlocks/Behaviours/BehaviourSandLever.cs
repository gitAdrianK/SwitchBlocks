namespace SwitchBlocks.Behaviours
{
    using System.Linq;
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Data;
    using SwitchBlocks.Settings;
    using SwitchBlocks.Util;

    /// <summary>
    /// Behaviour related to sand levers.
    /// </summary>
    public class BehaviourSandLever : IBlockBehaviour
    {
        private DataSand Data { get; }
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        public BehaviourSandLever() => this.Data = DataSand.Instance;

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
            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockSandLever>();
            var collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockSandLeverOn>();
            var collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockSandLeverOff>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockSandLeverSolid>();
            var collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOn>();
            var collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOff>();
            var collidingWithAnyLever = collidingWithLever || collidingWithLeverSolid;
            var collidingWithAnyLeverOn = collidingWithLeverOn || collidingWithLeverSolidOn;
            var collidingWithAnyLeverOff = collidingWithLeverOff || collidingWithLeverSolidOff;
            this.IsPlayerOnBlock = collidingWithAnyLever
                || collidingWithAnyLeverOn
                || collidingWithAnyLeverOff;

            if (this.IsPlayerOnBlock)
            {
                if (this.Data.HasSwitched)
                {
                    return true;
                }
                this.Data.HasSwitched = true;

                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
                {
                    var block = advCollisionInfo.GetCollidedBlocks().First(b =>
                    {
                        var type = b.GetType();
                        return type == typeof(BlockSandLeverSolid)
                        || type == typeof(BlockSandLeverSolidOn)
                        || type == typeof(BlockSandLeverSolidOff);
                    });
                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsSand.LeverDirections,
                        block))
                    {
                        return true;
                    }
                }

                var stateBefore = this.Data.State;
                if (collidingWithAnyLever)
                {
                    this.Data.State = !this.Data.State;
                }
                else if (collidingWithAnyLeverOn)
                {
                    this.Data.State = true;
                }
                else if (collidingWithAnyLeverOff)
                {
                    this.Data.State = false;
                }

                if (stateBefore != this.Data.State)
                {
                    ModSounds.SandFlip?.PlayOneShot();
                }
            }
            else
            {
                this.Data.HasSwitched = false;
            }
            return true;
        }
    }
}
