namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// The group B block.
    /// </summary>
    public class BlockGroupB : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockGroupB(Rectangle collider) : base(collider)
        {
        }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.GROUP_B;

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
