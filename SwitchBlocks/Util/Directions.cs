namespace SwitchBlocks.Util
{
    using System.Collections.Specialized;
    using Behaviours;
    using JumpKing.BodyCompBehaviours;
    using JumpKing.Level;

    /// <summary>Directions.</summary>
    public enum Direction : byte
    {
        Up = 0b0001,
        Down = 0b0010,
        Left = 0b0100,
        Right = 0b1000,
        All = 0b1111
    }

    /// <summary>Contains a way to determine the <see cref="Direction" /> a collision has happened from.</summary>
    public static class Directions
    {
        /// <summary>
        ///     Checks from with <see cref="Direction" /> the collision took place and if it is a valid direction.
        /// </summary>
        /// <param name="behaviourContext"><see cref="BehaviourContext" />.</param>
        /// <param name="validDirections">Valid directions.</param>
        /// <param name="block"><see cref="IBlock" /> that has its collision <see cref="Direction" /> checked.</param>
        /// <returns>
        ///     <c>true</c> if the collision <see cref="Direction" /> for the <see cref="IBlock" /> type is valid,
        ///     <c>false</c> otherwise.
        /// </returns>
        public static bool ResolveCollisionDirection(BehaviourContext behaviourContext, BitVector32 validDirections,
            IBlock block)
        {
            var prevVelocity = BehaviourPost.PrevVelocity;
            // The behaviour to save the prev velocity runs before any behaviour requiring the previous velocity.
            // A different name would be "CurrentVelocity".
            //var prevVelocity = behaviourContext.BodyComp.LastVelocity;
            var playerRect = behaviourContext.BodyComp.GetHitbox();
            var blockRect = block.GetRect();
            if (playerRect.Bottom - blockRect.Top == 0
                && prevVelocity.Y > 0.0f
                && validDirections[(int)Direction.Up])
            {
                return true;
            }

            if (blockRect.Bottom - playerRect.Top == 0
                && prevVelocity.Y <= 0.0f
                && validDirections[(int)Direction.Down])
            {
                return true;
            }

            if (playerRect.Right - blockRect.Left == 0
                && prevVelocity.X >= 0.0f
                && validDirections[(int)Direction.Left])
            {
                return true;
            }

            return blockRect.Right - playerRect.Left == 0
                   && prevVelocity.X <= 0.0f
                   && validDirections[(int)Direction.Right];
        }
    }
}
