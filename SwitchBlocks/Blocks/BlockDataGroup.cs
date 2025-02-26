namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// Abstract block that can also provide group data
    /// </summary>
    public abstract class BlockDataGroup : IBlock, IBlockDebugColor, IBlockGroupId
    {
        protected IGroupDataProvider Data { get; }
        protected Color Color { get; }

        protected Rectangle Collider { get; }

        public int GroupId { get; set; } = 0;

        protected BlockDataGroup(Rectangle collider, Color color, IGroupDataProvider data)
        {
            this.Collider = collider;
            this.Data = data;
            this.Color = color;
        }

        public Color DebugColor => this.Color;

        public Rectangle GetRect() => this.Data.GetState(this.GroupId) ? this.Collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                if (this.Data.GetState(this.GroupId))
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
