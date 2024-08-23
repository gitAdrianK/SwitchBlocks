using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;

namespace SwitchBlocks.Behaviours
{
    /// <summary>
    /// Behaviour related to auto reset blocks.
    /// </summary>
    public class BehaviourAutoReset : IBlockBehaviour
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

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            bool isReset = advCollisionInfo.IsCollidingWith<BlockAutoReset>();
            bool isResetFull = advCollisionInfo.IsCollidingWith<BlockAutoResetFull>();
            IsPlayerOnBlock = isReset || isResetFull;

            if (IsPlayerOnBlock)
            {
                if (isResetFull)
                {
                    DataAuto.State = false;
                }
                DataAuto.RemainingTime = ModBlocks.AutoDuration;
                DataAuto.WarnCount = 0;
            }

            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            return inputXVelocity;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            return inputYVelocity;
        }
    }
}
