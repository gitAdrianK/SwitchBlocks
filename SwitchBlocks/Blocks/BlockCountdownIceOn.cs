namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown ice on block.
    /// </summary>
    public class BlockCountdownIceOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownIceOn(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_ICE_ON;

        /// <inheritdoc/>
        public override Rectangle GetRect() => DataCountdown.Instance.State ? this.Collider : Rectangle.Empty;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                if (DataCountdown.Instance.State)
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
