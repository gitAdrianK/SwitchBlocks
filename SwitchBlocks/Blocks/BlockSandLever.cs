namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sand lever block.
    /// </summary>
    public class BlockSandLever : ModBlock
    {
        /// <inheritdoc/>
        public BlockSandLever(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SAND_LEVER;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
