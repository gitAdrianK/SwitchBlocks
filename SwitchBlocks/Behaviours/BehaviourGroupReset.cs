using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourGroupReset : IBlockBehaviour
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
            bool collidingWithReset = advCollisionInfo.IsCollidingWith<BlockGroupReset>();
            bool collidingWithResetSolid = advCollisionInfo.IsCollidingWith<BlockGroupResetSolid>();
            IsPlayerOnBlock = collidingWithReset || collidingWithResetSolid;
            if (!IsPlayerOnBlock)
            {
                DataGroup.HasSwitched = false;
                return true;
            }

            if (DataGroup.HasSwitched)
            {
                return true;
            }
            DataGroup.HasSwitched = true;

            // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
            if (collidingWithResetSolid)
            {
                IBlock block = advCollisionInfo.GetCollidedBlocks().First(b => b.GetType() == typeof(BlockGroupResetSolid));
                if (!Directions.ResolveCollisionDirection(behaviourContext,
                    SettingsGroup.LeverDirections,
                    block))
                {
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

            return true;
        }
    }
}
