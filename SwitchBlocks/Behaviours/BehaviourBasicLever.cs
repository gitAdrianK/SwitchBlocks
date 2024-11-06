using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;

namespace SwitchBlocks.Behaviours
{
    /// <summary>
    /// Behaviour related to basic levers.
    /// </summary>
    public class BehaviourBasicLever : IBlockBehaviour
    {
        public float BlockPriority => 2.0f;

        public bool IsPlayerOnBlock { get; set; }

        private Vector2 prevVelocity = new Vector2(0, 0);

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
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockBasicLever>();
            bool collidingWithLeverOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverOn>();
            bool collidingWithLeverOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverOff>();
            bool collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolid>();
            bool collidingWithLeverSolidOn = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOn>();
            bool collidingWithLeverSolidOff = advCollisionInfo.IsCollidingWith<BlockBasicLeverSolidOff>();
            bool collidingWithAnyLever = collidingWithLever || collidingWithLeverSolid;
            bool collidingWithAnyLeverOn = collidingWithLeverOn || collidingWithLeverSolidOn;
            bool collidingWithAnyLeverOff = collidingWithLeverOff || collidingWithLeverSolidOff;
            IsPlayerOnBlock = collidingWithAnyLever
                || collidingWithAnyLeverOn
                || collidingWithAnyLeverOff;

            if (IsPlayerOnBlock)
            {
                if (DataBasic.HasSwitched)
                {
                    prevVelocity = behaviourContext.BodyComp.Velocity;
                    return true;
                }
                DataBasic.HasSwitched = true;

                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid || collidingWithLeverSolidOn || collidingWithLeverSolidOff)
                {
                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                        prevVelocity,
                        SettingsBasic.LeverDirections,
                        typeof(BlockBasicLeverSolid),
                        typeof(BlockBasicLeverSolidOn),
                        typeof(BlockBasicLeverSolidOff)))
                    {
                        prevVelocity = behaviourContext.BodyComp.Velocity;
                        return true;
                    }
                }

                bool stateBefore = DataBasic.State;
                if (collidingWithAnyLever)
                {
                    DataBasic.State = !DataBasic.State;
                }
                else if (collidingWithAnyLeverOn)
                {
                    DataBasic.State = true;
                }
                else if (collidingWithAnyLeverOff)
                {
                    DataBasic.State = false;
                }

                if (stateBefore != DataBasic.State)
                {
                    ModSounds.BasicFlip?.PlayOneShot();
                }
            }
            else
            {
                DataBasic.HasSwitched = false;
            }
            prevVelocity = behaviourContext.BodyComp.Velocity;
            return true;
        }
    }
}
