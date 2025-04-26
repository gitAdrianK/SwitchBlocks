namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block.
    /// </summary>
    public class BlockBasicLever : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicLever(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_LEVER;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
