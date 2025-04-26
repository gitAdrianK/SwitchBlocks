namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sand lever block, capable of only turning the state off.
    /// </summary>
    public class BlockSandLeverOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockSandLeverOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SAND_LEVER_OFF;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
