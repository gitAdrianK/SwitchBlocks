using ErikMaths;
using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.MiscEntities.WorldItems;
using JumpKing.MiscEntities.WorldItems.Inventory;
using JumpKing.Player;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourCountdownIceOn : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }
        public static bool IsPlayerOnIce { get; set; }

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
            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockCountdownIceOn>();
            IsPlayerOnIce = IsPlayerOnBlock && DataCountdown.State && !InventoryManager.HasItemEnabled(Items.SnakeRing);
            if (IsPlayerOnIce)
            {
                BodyComp bodyComp = behaviourContext.BodyComp;
                bodyComp.Velocity.X = ErikMath.MoveTowards(bodyComp.Velocity.X, 0f, PlayerValues.ICE_FRICTION);
            }
            if (IsPlayerOnBlock && !DataCountdown.State)
            {
                Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
                List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockCountdownIceOn));
                foreach (IBlock block in blocks)
                {
                    block.Intersects(playerRect, out Rectangle collision);
                    if (collision.Size.X > 0 || collision.Size.Y > 0)
                    {
                        DataCountdown.CanSwitchSafely = false;
                        return true;
                    }
                }
            }

            return true;
        }
    }
}
