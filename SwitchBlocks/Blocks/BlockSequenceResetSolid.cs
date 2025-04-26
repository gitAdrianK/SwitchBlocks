namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sequence solid reset block.
    /// </summary>
    public class BlockSequenceResetSolid : ModBlock
    {
        /// <inheritdoc/>
        public BlockSequenceResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SEQUENCE_RESET_SOLID;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => true;
    }
}
