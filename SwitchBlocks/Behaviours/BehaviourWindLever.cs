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
    /// Behaviour related to wind levers.
    /// </summary>
    public class BehaviourWindLever : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        private DataWind Data { get; }
        public bool IsPlayerOnBlock { get; set; }

        public BehaviourWindLever() => this.Data = DataWind.Instance;

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
            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockWindLever>();
            var collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockWindLeverOn>();
            var collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockWindLeverOff>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockWindLeverSolid>();
            var collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockWindLeverSolidOn>();
            var collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockWindLeverSolidOff>();
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
                        return type == typeof(BlockWindLeverSolid)
                        || type == typeof(BlockWindLeverSolidOn)
                        || type == typeof(BlockWindLeverSolidOff);
                    });
                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsWind.LeverDirections,
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
                    ModSounds.WindFlip?.PlayOneShot();
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
