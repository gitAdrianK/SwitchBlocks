namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow on block.
    /// </summary>
    public class BlockAutoSnowOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockAutoSnowOn(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.AUTO_SNOW_ON;

        public Rectangle GetRect() => DataAuto.Instance.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataAuto.Instance.State)
                {
                    return BlockCollisionType.Collision_Blocking;
                }
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
