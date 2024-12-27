namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block.
    /// </summary>
    public class BlockBasicLeverSolid : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicLeverSolid(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.BASIC_LEVER_SOLID;

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
