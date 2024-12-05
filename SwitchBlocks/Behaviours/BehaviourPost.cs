using ErikMaths;
using JumpKing;
using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.MiscEntities.WorldItems;
using JumpKing.MiscEntities.WorldItems.Inventory;
using JumpKing.Player;

namespace SwitchBlocks.Behaviours
{
    public class BehaviourPost : IBlockBehaviour
    {
        // Documentation is false, higher numbers are run first!
        public float BlockPriority => 1.0f;

        public bool IsPlayerOnBlock { get; set; }

        public static bool IsPlayerOnIce { get; set; }
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

            // TODO: Without testing, but pretty sure its that way, both vanilla ice and this slide apply
            // resulting in the player slowing down twice as fast.
            // Figure out a way to make them play nice.
            // -> Test if advCI.Ice works
            AdvCollisionInfo advCollisionInfo = behaviourContext.CollisionInfo.PreResolutionCollisionInfo;
            if (IsPlayerOnIce
                && !advCollisionInfo.Ice
                && !InventoryManager.HasItemEnabled(Items.SnakeRing))
            {
                BodyComp bodyComp = behaviourContext.BodyComp;
                bodyComp.Velocity.X = ErikMath.MoveTowards(bodyComp.Velocity.X, 0f, PlayerValues.ICE_FRICTION);
            }

            return true;
        }
    }
}
