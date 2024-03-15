using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;
using SwitchBlocksMod.Util;

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
            if (advCollisionInfo.IsCollidingWith<BlockBasicLeverSolid>()
                || advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOn>()
                || advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOff>())
            {
                if (DataBasic.HasSwitched)
                {
                    return true;
                }
                DataBasic.HasSwitched = true;

                bool stateBefore = DataBasic.State;
                if (advCollisionInfo.IsCollidingWith<BlockBasicLeverSolid>())
                {
                    DataBasic.State = !DataBasic.State;
                }
                else if (advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOn>())
                {
                    DataBasic.State = true;
                }
                else if (advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOff>())
                {
                    DataBasic.State = false;
                }

                if (ModSounds.BASIC_FLIP != null && stateBefore != DataBasic.State)
                {
                    ModSounds.BASIC_FLIP.Play();
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
