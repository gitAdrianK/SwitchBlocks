using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System;
using System.Threading.Tasks;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourGroupReset : IBlockBehaviour
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
            bool collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupReset>();
            bool collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupResetSolid>();
            IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
            if (!IsPlayerOnBlock)
            {
                DataGroup.HasSwitched = false;
                prevVelocity = behaviourContext.BodyComp.Velocity;
                return true;
            }

            if (DataGroup.HasSwitched)
            {
                prevVelocity = behaviourContext.BodyComp.Velocity;
                return true;
            }
            DataGroup.HasSwitched = true;

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithResetSolid)
            {
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                    prevVelocity,
                    SettingsGroup.LeverDirections,
                    typeof(BlockGroupResetSolid)))
                {
                    prevVelocity = behaviourContext.BodyComp.Velocity;
                    return true;
                }
            }

            Parallel.ForEach(DataGroup.Active, group =>
            {
                DataGroup.Groups[group].ActivatedTick = Int32.MaxValue;
            });
            Parallel.ForEach(DataGroup.Finished, group =>
            {
                DataGroup.Groups[group].ActivatedTick = Int32.MaxValue;
                DataGroup.Active.Add(group);
            });
            DataGroup.Finished.Clear();
            DataGroup.Touched.Clear();

            prevVelocity = behaviourContext.BodyComp.Velocity;
            return true;
        }
    }
}
