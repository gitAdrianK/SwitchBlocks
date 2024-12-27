namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// The sequence snow D block.
    /// </summary>
    public class BlockSequenceSnowD : IBlock, IBlockDebugColor, IBlockGroupId
    {
        public int GroupId { get; set; } = 0;

        private readonly Rectangle collider;

        public BlockSequenceSnowD(Rectangle collider) => this.collider = collider;

        public Color DebugColor => ModBlocks.SEQUENCE_SNOW_D;

        public Rectangle GetRect() => DataSequence.GetState(this.GroupId) ? this.collider : Rectangle.Empty;

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (this.collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, this.collider);
                if (DataSequence.GetState(this.GroupId))
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
