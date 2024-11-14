using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.MiscEntities.WorldItems;
using JumpKing.MiscEntities.WorldItems.Inventory;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourAutoSnow : IBlockBehaviour
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
            bool isPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockAutoSnowOn>();
            bool isPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockAutoSnowOff>();
            IsPlayerOnBlock = isPlayerOnBlockOn || isPlayerOnBlockOff;

            if (!IsPlayerOnBlock)
            {
                IsPlayerOnSnow = false;
                return true;
            }

            IsPlayerOnSnow = !InventoryManager.HasItemEnabled(Items.SnakeRing)
                && ((isPlayerOnBlockOn && DataAuto.State) || (isPlayerOnBlockOff && !DataAuto.State));

            return true;
        }
    }
}
