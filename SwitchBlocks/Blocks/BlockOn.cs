namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// Block representing all on blocks
    /// </summary>
    public abstract class BlockOn : BlockData
    {
        public BlockOn(Rectangle rectangle, Color color, IDataProvider data) : base(rectangle, color, data)
        {
        }

        public override Rectangle GetRect() => this.Data.State ? this.Collider : Rectangle.Empty;

        public override BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.Collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.Collider);
                if (this.Data.State)
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
