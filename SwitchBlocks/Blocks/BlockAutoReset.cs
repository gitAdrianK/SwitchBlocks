namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The auto reset block.
    /// </summary>
    public class BlockAutoReset : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.AUTO_RESET;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
