namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sequence reset block.
    /// </summary>
    public class BlockSequenceReset : ModBlock
    {
        /// <inheritdoc/>
        public BlockSequenceReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SEQUENCE_RESET;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
