namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto off block.
    /// </summary>
    public class BlockAutoOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockAutoOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.AUTO_OFF;

        public Rectangle GetRect() => !DataAuto.Instance.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataAuto.Instance.State)
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
