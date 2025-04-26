namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The auto reset full block.
    /// </summary>
    public class BlockAutoResetFull : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoResetFull(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.AUTO_RESET_FULL;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
