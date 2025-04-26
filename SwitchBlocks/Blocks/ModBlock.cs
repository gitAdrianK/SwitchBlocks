namespace SwitchBlocks.Blocks
{
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    public abstract class ModBlock : BoxBlock, IBlockDebugColor
    {
        /// <summary>
        /// Ctor.
        /// </summary>
        /// <param name="collider">Collider to be used for this block.</param>
        protected ModBlock(Rectangle collider) : base(collider) { }

        /// <summary>
        /// <see cref="Color"/> this block is drawn as when in debug.
        /// </summary>
        public abstract Color DebugColor { get; }

        /// <summary>
        /// If this block is solid.
        /// </summary>
        public abstract bool CanBlockPlayer { get; }

        /// <summary>
        /// Override of the <see cref="BoxBlock"/> property,
        /// To force mod blocks to implement it, naming, and documentation.
        /// </summary>
        protected override bool canBlockPlayer => this.CanBlockPlayer;
    }
}
