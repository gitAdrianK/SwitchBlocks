namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    public class BlockGroupReset : IBlock, IBlockDebugColor, IResetGroupIds
    {
        public int[] ResetIds { get; set; } = { };

        private readonly Rectangle collider;

        public BlockGroupReset(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.GROUP_RESET;

        public Rectangle GetRect() => this.collider;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
