namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// Abstract block that can also provide data
    /// </summary>
    public abstract class BlockData : IBlock, IBlockDebugColor
    {
        protected IDataProvider Data { get; }
        protected Color Color { get; }

        protected Rectangle Collider { get; }

        protected BlockData(Rectangle collider, Color color, IDataProvider data)
        {
            this.Collider = collider;
            this.Data = data;
            this.Color = color;
        }

        public Color DebugColor => this.Color;

        public abstract Rectangle GetRect();

        public abstract BlockCollisionType Intersects(Rectangle p_hitbox, out Rectangle p_intersection);
    }
}
