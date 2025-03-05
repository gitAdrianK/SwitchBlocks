namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpIceOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockJumpIceOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.JUMP_ICE_OFF;

        public Rectangle GetRect() => !DataJump.Instance.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataJump.Instance.State)
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
