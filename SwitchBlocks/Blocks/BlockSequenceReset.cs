namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockSequenceReset : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockSequenceReset(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.SEQUENCE_RESET;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
