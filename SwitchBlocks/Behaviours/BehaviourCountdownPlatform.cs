using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourCountdownPlatform : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }
        public static bool CanSwitchSafely { get; private set; } = true;

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
            bool isPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockCountdownOn>();
            bool isPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockCountdownOff>();
            IsPlayerOnBlock = isPlayerOnBlockOn || isPlayerOnBlockOff;
            CanSwitchSafely = true;
            if ((isPlayerOnBlockOff && DataCountdown.State) || (isPlayerOnBlockOn && !DataCountdown.State))
            {
                CanSwitchSafely = false;
            }

            return true;
        }
    }
}
