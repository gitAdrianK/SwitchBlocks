using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using SwitchBlocks.Blocks;
using SwitchBlocks.Data;
using SwitchBlocks.Patching;
using SwitchBlocks.Settings;
using SwitchBlocks.Util;
using System.Linq;

namespace SwitchBlocks.Behaviours
{
    /// <summary>
    /// Behaviour related to countdown levers.
    /// </summary>
    public class BehaviourCountdownLever : IBlockBehaviour
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
            bool collidingWithLever = advCollisionInfo.IsCollidingWith<BlockCountdownLever>();
            bool collidingWithLeverSolid = advCollisionInfo.IsCollidingWith<BlockCountdownLeverSolid>();
            IsPlayerOnBlock = collidingWithLever || collidingWithLeverSolid;

            if (IsPlayerOnBlock)
            {
                // The collision is jank for the non-solid levers, so for now I'll limit this feature to the solid ones
                if (collidingWithLeverSolid)
                {
                    IBlock block = advCollisionInfo.GetCollidedBlocks().First(b => b.GetType() == typeof(BlockCountdownLeverSolid));
                    if (!Directions.ResolveCollisionDirection(behaviourContext,
                        SettingsCountdown.LeverDirections,
                        block))
                    {
                        return true;
                    }
                }

                DataCountdown.ActivatedTick = AchievementManager.GetTicks();

                if (DataCountdown.HasSwitched)
                {
                    return true;
                }

                if (!DataCountdown.State)
                {
                    ModSounds.CountdownFlip?.PlayOneShot();
                }

                DataCountdown.HasSwitched = true;
                DataCountdown.State = true;
            }
            else
            {
                DataCountdown.HasSwitched = false;
            }
            return true;
        }
    }
}
