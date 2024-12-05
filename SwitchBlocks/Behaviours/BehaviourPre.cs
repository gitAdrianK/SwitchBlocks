using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Data;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourPre : IBlockBehaviour
    {
        // Documentation is false, higher numbers are run first!
        public float BlockPriority => 3.0f;

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
            DataAuto.CanSwitchSafely = true;
            DataCountdown.CanSwitchSafely = true;
            DataJump.CanSwitchSafely = true;

            BehaviourPost.IsPlayerOnIce = false;
            BehaviourPost.IsPlayerOnSnow = false;

            return true;
        }
    }
}
