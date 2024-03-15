using JumpKing.API;
using JumpKing.BodyCompBehaviours;
using JumpKing.Level;
using JumpKing.Player;

//TODO: Sand Behaviour
namespace SwitchBlocksMod.Behaviours
{
    /// <summary>
    /// Behaviour related to sand.
    /// </summary>
    public class BehaviourSand : IBlockBehaviour
    {
        private readonly ICollisionQuery m_collisionQuery;

        public float BlockPriority => 1f;

        public bool IsPlayerOnBlock { get; set; }

        public float ModifyXVelocity(float inputXVelocity, BehaviourContext behaviourContext)
        {
            //TODO: I don't think the X velocity needs to be changed
            float num = (IsPlayerOnBlock ? 0.25f : 1f);
            return inputXVelocity * num;
        }

        public float ModifyYVelocity(float inputYVelocity, BehaviourContext behaviourContext)
        {
            //TODO: Change Y velocity based on state
            BodyComp bodyComp = behaviourContext.BodyComp;
            float num = ((IsPlayerOnBlock && bodyComp.Velocity.Y <= 0f) ? 0.5f : 1f);
            float result = inputYVelocity * num;
            if (!IsPlayerOnBlock && bodyComp.IsOnGround && bodyComp.Velocity.Y > 0f)
            {
                bodyComp.Position.Y += 1f;
            }

            return result;
        }

        public bool AdditionalXCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            //TODO: Harmony it up, I guess
            if (info.Sand)
            {
                return !IsPlayerOnBlock;
            }

            return false;
        }

        public bool AdditionalYCollisionCheck(AdvCollisionInfo info, BehaviourContext behaviourContext)
        {
            if (info.Sand && !IsPlayerOnBlock)
            {
                return behaviourContext.BodyComp.Velocity.Y < 0f;
            }

            return false;
        }

        public bool ExecuteBlockBehaviour(BehaviourContext behaviourContext)
        {
            /*
            BodyComp bodyComp = behaviourContext.BodyComp;
            Rectangle hitbox = bodyComp.GetHitbox();
            m_collisionQuery.CheckCollision(hitbox, out Rectangle _, out AdvCollisionInfo info);
            IsPlayerOnBlock = info.Sand;
            if (IsPlayerOnBlock)
            {
                bodyComp.Velocity.Y = Math.Min(0.75f, bodyComp.Velocity.Y);
                //bodyComp._knocked = false;
            }
            */
            return true;
        }

        public float ModifyGravity(float inputGravity, BehaviourContext behaviourContext)
        {
            return inputGravity;
        }
    }
}