namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic ice on block.
    /// </summary>
    public class BlockBasicIceOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicIceOn(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.BASIC_ICE_ON;

        public Rectangle GetRect() => DataBasic.Instance.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataBasic.Instance.State)
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
