namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow off block.
    /// </summary>
    public class BlockAutoSnowOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockAutoSnowOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.AUTO_SNOW_OFF;

        public Rectangle GetRect() => !DataAuto.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataAuto.State)
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
