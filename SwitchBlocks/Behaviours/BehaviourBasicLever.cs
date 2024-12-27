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
    /// Behaviour related to basic levers.
    /// </summary>
    public class BehaviourBasicLever : IBlockBehaviour
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
            var collidingWithLever = advCollisionInfo.IsCollidingWith<BlockBasicLever>();
            var collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverOn>();
            var collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverOff>();
            var collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolid>();
            var collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOn>();
            var collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOff>();
            var collidingWithAnyLever = collidingWithLever || collidingWithLeverSolid;
            var collidingWithAnyLeverOn = collidingWithLeverOn || collidingWithLeverSolidOn;
            var collidingWithAnyLeverOff = collidingWithLeverOff || collidingWithLeverSolidOff;
            this.IsPlayerOnBlock = collidingWithAnyLever
                || collidingWithAnyLeverOn
                || collidingWithAnyLeverOff;

            if (this.IsPlayerOnBlock)
            {
                if (DataBasic.HasSwitched)
                {
                    return true;
                }
                DataBasic.HasSwitched = true;

                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
                {
                    var block = advCollisionInfo.GetCollidedBlocks().First(b =>
                    {
                        var type = b.GetType();
                        return type == typeof(BlockBasicLeverSolid)
                        || type == typeof(BlockBasicLeverSolidOn)
                        || type == typeof(BlockBasicLeverSolidOff);
                    });
                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsBasic.LeverDirections,
                        block))
                    {
                        return true;
                    }
                }

                var stateBefore = DataBasic.State;
                if (collidingWithAnyLever)
                {
                    DataBasic.State = !DataBasic.State;
                }
                else if (collidingWithAnyLeverOn)
                {
                    DataBasic.State = true;
                }
                else if (collidingWithAnyLeverOff)
                {
                    DataBasic.State = false;
                }

                if (stateBefore != DataBasic.State)
                {
                    ModSounds.BasicFlip?.PlayOneShot();
                }
            }
            else
            {
                DataBasic.HasSwitched = false;
            }
            return true;
        }
    }
}
