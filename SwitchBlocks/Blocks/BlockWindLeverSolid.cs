namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The wind lever block.
    /// </summary>
    public class BlockWindLeverSolid : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockWindLeverSolid(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.WIND_LEVER_SOLID;

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
