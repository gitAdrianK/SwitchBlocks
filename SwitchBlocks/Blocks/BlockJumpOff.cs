namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockJumpOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.JUMP_OFF;

        public Rectangle GetRect() => !DataJump.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataJump.State)
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
