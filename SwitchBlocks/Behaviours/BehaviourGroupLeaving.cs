using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourGroupLeaving : IBlockBehaviour
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
            IsPlayerOnBlock = advCollisionInfo.IsCollidingWith<BlockGroupA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowA>()
                || advCollisionInfo.IsCollidingWith<BlockGroupB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupIceB>()
                || advCollisionInfo.IsCollidingWith<BlockGroupSnowB>();

            int tick = AchievementManager.GetTicks();
            if (!IsPlayerOnBlock)
            {
                Parallel.ForEach(DataGroup.Touched, id =>
                {
                    Debugger.Log(1, "", ">>> Left all blocks, writing tick to " + id + "\n");
                    DataGroup.SetTick(id, tick);
                });
                DataGroup.Touched.Clear();
                return true;
            }

            List<IBlock> blocks = advCollisionInfo.GetCollidedBlocks().ToList().FindAll(b => b.GetType() == typeof(BlockGroupA)
                || b.GetType() == typeof(BlockGroupIceA)
                || b.GetType() == typeof(BlockGroupSnowA)
                || b.GetType() == typeof(BlockGroupB)
                || b.GetType() == typeof(BlockGroupIceB)
                || b.GetType() == typeof(BlockGroupSnowB));
            HashSet<int> currentlyTouched = new HashSet<int>();
            foreach (IBlockGroupId block in blocks.Cast<IBlockGroupId>())
            {
                int groupId = block.GroupId;
                if (!DataGroup.GetState(groupId))
                {
                    continue;
                }
                if (!DataGroup.Touched.Contains(groupId))
                {
                    Debugger.Log(1, "", ">>> Entered block, adding " + groupId + " to touched\n");
                }
                currentlyTouched.Add(groupId);
            }

            foreach (int id in DataGroup.Touched.Except(currentlyTouched))
            {
                Debugger.Log(1, "", ">>> No longer touching " + id + "\n");
            }

            Parallel.ForEach(DataGroup.Touched.Except(currentlyTouched), id =>
            {
                Debugger.Log(1, "", ">>> Left some blocks, writing tick to " + id + "\n");
                DataGroup.SetTick(id, tick);
            });

            DataGroup.Touched = currentlyTouched;

            return true;
        }
    }
}