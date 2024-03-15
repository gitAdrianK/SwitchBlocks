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
        public float BlockPriority => 2f;

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
            if (advCollisionInfo.IsCollidingWith<BlockSandLeverSolid>()
                || advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOn>()
                || advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOff>())
            {
                if (DataSand.HasSwitched)
                {
                    return true;
                }
                DataSand.HasSwitched = true;

                bool stateBefore = DataSand.State;
                if (advCollisionInfo.IsCollidingWith<BlockSandLeverSolid>())
                {
                    DataSand.State = !DataSand.State;
                }
                else if (advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOn>())
                {
                    DataSand.State = true;
                }
                else if (advCollisionInfo.IsCollidingWith<BlockSandLeverSolidOff>())
                {
                    DataSand.State = false;
                }

                if (ModSounds.SAND_FLIP != null && stateBefore != DataSand.State)
                {
                    ModSounds.SAND_FLIP.Play();
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
