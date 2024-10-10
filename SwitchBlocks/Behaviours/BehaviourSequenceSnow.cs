using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.MiscEntities.WorldItems;
using JumpKing.MiscEntities.WorldItems.Inventory;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Util;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourSequenceSnow : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        public static bool IsPlayerOnSnow { get; set; }

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
            bool isPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockSequenceSnowA>();
            bool isPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockSequenceSnowB>();
            IsPlayerOnBlock = isPlayerOnBlockOn || isPlayerOnBlockOff;

            if (!IsPlayerOnBlock || InventoryManager.HasItemEnabled(Items.SnakeRing))
            {
                IsPlayerOnSnow = false;
                return true;
            }

            List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockSequenceSnowA)
                || b.GetType() == typeof(BlockSequenceSnowB));
            foreach (IBlockGroupId block in blocks.Cast<IBlockGroupId>())
            {
                if (DataSequence.GetState(block.GroupId))
                {
                    IsPlayerOnSnow = true;
                    break;
                }
            }

            return true;
        }
    }
}