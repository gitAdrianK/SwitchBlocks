using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourAutoOn : IBlockBehaviour
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
            bool isOnBasic = advCollisionInfo.IsCollidingWith<BlockAutoOn>();
            bool isOnIce = advCollisionInfo.IsCollidingWith<BlockAutoIceOn>();
            bool isOnSnow = advCollisionInfo.IsCollidingWith<BlockAutoSnowOn>();
            IsPlayerOnBlock = isOnBasic || isOnIce || isOnSnow;
            if (!IsPlayerOnBlock)
            {
                return true;
            }

            if (DataAuto.State)
            {
                if (isOnIce)
                {
                    BehaviourPost.IsPlayerOnIce = true;
                }

                if (isOnSnow)
                {
                    BehaviourPost.IsPlayerOnSnow = true;
                }
            }
            else
            {
                if (DataAuto.CanSwitchSafely)
                {
                    DataAuto.CanSwitchSafely = !Intersecting.IsIntersectingBlocks(
                        behaviourContext,
                        typeof(BlockAutoOn),
                        typeof(BlockAutoIceOn),
                        typeof(BlockAutoSnowOn));
                }
            }

            return true;
        }
    }
}
