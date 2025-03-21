namespace SwitchBlocks.Util
{
    using System.Collections.Specialized;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;
    using SwitchBlocks.Behaviours;

    public static class Directions
    {
        public enum Direction : byte
        {
            Up = 0b0001,
            Down = 0b0010,
            Left = 0b0100,
            Right = 0b1000,
            All = 0b1111,
        }

        /// <summary>
        /// Checks from with direction the collision took place and if its a valid direction.
        /// </summary>
        /// <param name="behaviourContext">The behaviour context</param>
        /// <param name="validDirections">Valid directions</param>
        /// <param name="block">The block that has its direction checked</param>
        /// <returns>true if the direction for the block type is valid, otherwise, false</returns>
        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, BitVector32 validDirections, IBlock block)
        {
            var prevVelocity = BehaviourPost.PrevVelocity;
            var playerRect = behaviourContext.BodyComp.GetHitbox();
            var blockRect = block.GetRect();
            if (playerRect.Bottom - blockRect.Top == 0 && prevVelocity.Y > 0.0f && validDirections[(int)Direction.Up])
            {
                return true;
            }
            else if (blockRect.Bottom - playerRect.Top == 0 && prevVelocity.Y < 0.0f && validDirections[(int)Direction.Down])
            {
                return true;
            }
            else if (playerRect.Right - blockRect.Left == 0 && prevVelocity.X > 0.0f && validDirections[(int)Direction.Left])
            {
                return true;
            }
            else if (blockRect.Right - playerRect.Left == 0 && prevVelocity.X < 0.0f && validDirections[(int)Direction.Right])
            {
                return true;
            }
            return false;
        }
    }
}
