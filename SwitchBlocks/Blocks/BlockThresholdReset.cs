namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold reset block.
    /// </summary>
    public class BlockThresholdReset : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.ThresholdReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
