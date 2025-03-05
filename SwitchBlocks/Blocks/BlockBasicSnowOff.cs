namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic snow off block.
    /// </summary>
    public class BlockBasicSnowOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicSnowOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.BASIC_SNOW_OFF;

        public Rectangle GetRect() => !DataBasic.Instance.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataBasic.Instance.State)
                {
                    return BlockCollisionType.Collision_NonBlocking;
                }
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
