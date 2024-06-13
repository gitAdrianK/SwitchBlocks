using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to basic levers.
    /// </summary>
    public class BehaviourBasicLever : IBlockBehaviour
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
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockBasicLever>()
                || advCollisionInfo.IsCollidingWith<BlockBasicLeverSolid>();
            bool collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverOn>()
                || advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOn>();
            bool collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverOff>()
                || advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOff>();

            if (collidingWithLever || collidingWithLeverOn || collidingWithLeverOff)
            {
                if (DataBasic.HasSwitched)
                {
                    return true;
                }
                DataBasic.HasSwitched = true;

                bool stateBefore = DataBasic.State;
                if (collidingWithLever)
                {
                    DataBasic.State = !DataBasic.State;
                }
                else if (collidingWithLeverOn)
                {
                    DataBasic.State = true;
                }
                else if (collidingWithLeverOff)
                {
                    DataBasic.State = false;
                }

                if (stateBefore != DataBasic.State)
                {
                    ModSounds.basicFlip?.Play();
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
