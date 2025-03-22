namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow on block.
    /// </summary>
    public class BlockAutoSnowOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoSnowOn(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.AUTO_SNOW_ON;

        /// <inheritdoc/>
        public override Rectangle GetRect() => DataAuto.Instance.State ? this.Ccollider : Rectangle.Empty;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Ccollider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Ccollider);
                if (DataAuto.Instance.State)
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
