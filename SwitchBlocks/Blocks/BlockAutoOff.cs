namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto off block.
    /// </summary>
    public class BlockAutoOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoOff(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.AUTO_OFF;

        /// <inheritdoc/>
        public override Rectangle GetRect() => !DataAuto.Instance.State ? this.Ccollider : Rectangle.Empty;

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Ccollider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Ccollider);
                if (DataAuto.Instance.State)
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
