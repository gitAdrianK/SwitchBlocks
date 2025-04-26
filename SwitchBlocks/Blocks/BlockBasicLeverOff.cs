namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic lever block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicLeverOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicLeverOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_LEVER_OFF;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
