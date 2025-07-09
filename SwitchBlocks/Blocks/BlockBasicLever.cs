namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic lever block.
    /// </summary>
    public class BlockBasicLever : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicLever(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicLever;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
