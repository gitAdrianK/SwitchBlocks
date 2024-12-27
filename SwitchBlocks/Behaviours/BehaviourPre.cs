namespace SwitchBlocks.Behaviours
{
    using JumpKing.API;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Data;

    public class BehaviourPre : IBlockBehaviour
    {
        // Documentation is false, higher numbers are run first!
        public float BlockPriority => 3.0f;

        public bool IsPlayerOnBlock { get; set; }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext) => false;

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext) => inputGravity;

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext) => inputXVelocity;

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext) => inputYVelocity;

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
