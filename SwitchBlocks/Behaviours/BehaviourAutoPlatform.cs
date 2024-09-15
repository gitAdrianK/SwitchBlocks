using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourAutoPlatform : IBlockBehaviour
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
            bool isPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockAutoOn>();
            bool isPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockAutoOff>();
            IsPlayerOnBlock = isPlayerOnBlockOn || isPlayerOnBlockOff;
            DataAuto.CanSwitchSafely = true;
            if (!IsPlayerOnBlock)
            {
                return true;
            }

            if (isPlayerOnBlockOn && !DataAuto.State)
            {
                Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
                List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockAutoOn));
                foreach (IBlock block in blocks)
                {
                    block.Intersects(playerRect, out Rectangle collision);
                    if (collision.Size.X > 0 || collision.Size.Y > 0)
                    {
                        DataAuto.CanSwitchSafely = false;
                        return true;
                    }
                }
            }
            if (isPlayerOnBlockOff && DataAuto.State)
            {
                Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
                List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockAutoOff));
                foreach (IBlock block in blocks)
                {
                    block.Intersects(playerRect, out Rectangle collision);
                    if (collision.Size.X > 0 || collision.Size.Y > 0)
                    {
                        DataAuto.CanSwitchSafely = false;
                        return true;
                    }
                }
            }

            return true;
        }
    }
}
