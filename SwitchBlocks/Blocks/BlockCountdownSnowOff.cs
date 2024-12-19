namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown snow off block.
    /// </summary>
    public class BlockCountdownSnowOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockCountdownSnowOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.COUNTDOWN_SNOW_OFF;

        public Rectangle GetRect() => !DataCountdown.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataCountdown.State)
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
