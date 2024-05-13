using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;

namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to sand levers.
    /// </summary>
    public class BehaviourSandLever : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            return false;
        }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            return inputXVelocity;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            return inputYVelocity;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockSandLever>()
                || advCollisionInfo.IsCollidingWith<BlockSandLeverSolid>();
            bool collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockSandLeverOn>()
                || advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOn>();
            bool collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockSandLeverOff>()
                || advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOff>();

            if (collidingWithLever || collidingWithLeverOn || collidingWithLeverOff)
            {
                if (DataSand.HasSwitched)
                {
                    return true;
                }
                DataSand.HasSwitched = true;

                bool stateBefore = DataSand.State;
                if (collidingWithLever)
                {
                    DataSand.State = !DataSand.State;
                }
                else if (collidingWithLeverOn)
                {
                    DataSand.State = true;
                }
                else if (collidingWithLeverOff)
                {
                    DataSand.State = false;
                }

                if (stateBefore != DataSand.State)
                {
                    ModSounds.sandFlip?.Play();
                }
            }
            else
            {
                DataSand.HasSwitched = false;
            }
            return true;
        }
    }
}
