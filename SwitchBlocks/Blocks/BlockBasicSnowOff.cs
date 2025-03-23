namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic snow off block.
    /// </summary>
    public class BlockBasicSnowOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicSnowOff(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_SNOW_OFF;

        /// <inheritdoc/>
        public override Rectangle GetRect() => !DataBasic.Instance.State ? this.Collider : Rectangle.Empty;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                if (DataBasic.Instance.State)
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
