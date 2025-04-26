namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sand off block.
    /// </summary>
    public class BlockSandOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockSandOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SAND_OFF;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
