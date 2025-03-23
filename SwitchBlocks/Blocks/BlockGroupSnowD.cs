namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// The group snow D block.
    /// </summary>
    public class BlockGroupSnowD : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockGroupSnowD(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.GROUP_SNOW_D;

        /// <inheritdoc/>
        public override Rectangle GetRect()
        {
            if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group)
                && group.State)
            {
                return this.Collider;
            }
            return Rectangle.Empty;
        }

        /// <inheritdoc/>
        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group) && group.State)
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
