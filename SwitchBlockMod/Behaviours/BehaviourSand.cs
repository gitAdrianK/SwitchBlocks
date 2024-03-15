using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;
using SwitchBlocksMod.Blocks;
using SwitchBlocksMod.Data;

//TODO: Sand Behaviour
namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to sand.
    /// </summary>
    public class BehaviourSand : IBlockBehaviour
    {
        public float BlockPriority => 1.0f;

        public bool IsPlayerOnBlock { get; set; }

        public bool IsPlayerOnBlockOn { get; set; }
        public bool IsPlayerOnBlockOff { get; set; }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            float num = IsPlayerOnBlock ? 0.25f : 1.0f;
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            //TODO: Y Velocity
            return inputYVelocity;
        }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if ((info.IsCollidingWith<BlockSandOn>() || info.IsCollidingWith<BlockSandOff>()))
            {
                return !IsPlayerOnBlock;
            }
            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            //TODO: See about the player snapping to the top.
            BodyComp bodyComp = behaviourContext.BodyComp;
            if ((DataSand.State && info.IsCollidingWith<BlockSandOn>()) || (!DataSand.State && info.IsCollidingWith<BlockSandOff>()))
            {
                return bodyComp.Velocity.Y >= 0.0f;
            }
            else if ((info.IsCollidingWith<BlockSandOn>() || info.IsCollidingWith<BlockSandOff>()) && !IsPlayerOnBlock)
            {
                return bodyComp.Velocity.Y < 0.0f;
            }
            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            if (behaviourContext?.CollisionInfo?.PreResolutionCollisionInfo == null)
            {
                return true;
            }

            BodyComp bodyComp = behaviourContext.BodyComp;
            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;

            IsPlayerOnBlockOn = advCollisionInfo.IsCollidingWith<BlockSandOn>();
            IsPlayerOnBlockOff = advCollisionInfo.IsCollidingWith<BlockSandOff>();
            IsPlayerOnBlock = IsPlayerOnBlockOn || IsPlayerOnBlockOff;

            // TODO_HI: Cant jump while inside the block.
            //bodyComp._knocked = false;
            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }
    }
}