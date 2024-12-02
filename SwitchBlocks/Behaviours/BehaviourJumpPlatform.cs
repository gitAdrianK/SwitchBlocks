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
    public class BehaviourJumpPlatform : IBlockBehaviour
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
            bool isPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockJumpOn>()
                || advCollisionInfo.IsCollidingWith<BlockJumpIceOn>()
                || advCollisionInfo.IsCollidingWith<BlockJumpSnowOn>();

            bool isPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockJumpOff>()
                || advCollisionInfo.IsCollidingWith<BlockJumpIceOff>()
                || advCollisionInfo.IsCollidingWith<BlockJumpSnowOff>();

            IsPlayerOnBlock = isPlayerOnBlockOn || isPlayerOnBlockOff;
            DataJump.CanSwitchSafely = true;

            if (!IsPlayerOnBlock)
            {
                return true;
            }

            if (behaviourContext.BodyComp.IsOnGround)
            {
                DataJump.SwitchOnceSafe = false;
            }

            if (isPlayerOnBlockOn && !DataJump.State)
            {
                Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
                List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockJumpOn)
                || b.GetType() == typeof(BlockJumpIceOn)
                || b.GetType() == typeof(BlockJumpSnowOn));
                foreach (IBlock block in blocks)
                {
                    block.Intersects(playerRect, out Rectangle collision);
                    if (collision.Size.X > 0 || collision.Size.Y > 0)
                    {
                        DataJump.CanSwitchSafely = false;
                        break;
                    }
                }
            }
            else if (isPlayerOnBlockOff && DataJump.State)
            {
                Rectangle playerRect = behaviourContext.BodyComp.GetHitbox();
                List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockJumpOff)
                || b.GetType() == typeof(BlockJumpIceOff)
                || b.GetType() == typeof(BlockJumpSnowOff));
                foreach (IBlock block in blocks)
                {
                    block.Intersects(playerRect, out Rectangle collision);
                    if (collision.Size.X > 0 || collision.Size.Y > 0)
                    {
                        DataJump.CanSwitchSafely = false;
                        break;
                    }
                }
            }
            return true;
        }
    }
}
