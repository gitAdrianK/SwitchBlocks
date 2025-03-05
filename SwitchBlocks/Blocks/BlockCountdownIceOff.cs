namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown ice off block.
    /// </summary>
    public class BlockCountdownIceOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockCountdownIceOff(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.COUNTDOWN_ICE_OFF;

        public Rectangle GetRect() => !DataCountdown.Instance.State ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataCountdown.Instance.State)
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
