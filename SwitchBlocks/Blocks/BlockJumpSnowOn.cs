namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpSnowOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockJumpSnowOn(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.JUMP_SNOW_ON;

        public Rectangle GetRect() => DataJump.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataJump.State)
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
