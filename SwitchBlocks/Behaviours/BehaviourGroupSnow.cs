using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourGroupSnow : IBlockBehaviour
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
            bool isPlayerOnBlockA = advCollisionInfo.IsCollidingWith<BlockGroupSnowA>();
            bool isPlayerOnBlockB = advCollisionInfo.IsCollidingWith<BlockGroupSnowB>();
            bool isPlayerOnBlockC = advCollisionInfo.IsCollidingWith<BlockGroupSnowC>();
            bool isPlayerOnBlockD = advCollisionInfo.IsCollidingWith<BlockGroupSnowD>();
            IsPlayerOnBlock = isPlayerOnBlockA
                || isPlayerOnBlockB
                || isPlayerOnBlockC
                || isPlayerOnBlockD;

            if (!IsPlayerOnBlock)
            {
                return true;
            }

            List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockGroupSnowA)
                || b.GetType() == typeof(BlockGroupSnowB)
                || b.GetType() == typeof(BlockGroupSnowC)
                || b.GetType() == typeof(BlockGroupSnowD));
            foreach (IBlockGroupId block in blocks.Cast<IBlockGroupId>())
            {
                if (DataGroup.GetState(block.GroupId))
                {
                    BehaviourPost.IsPlayerOnSnow = true;
                    break;
                }
            }

            return true;
        }
    }
}
