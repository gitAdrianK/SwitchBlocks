namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public class BlockSequenceResetSolid : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockSequenceResetSolid(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.SEQUENCE_RESET_SOLID;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
