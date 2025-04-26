namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicLeverOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicLeverOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_LEVER_ON;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
