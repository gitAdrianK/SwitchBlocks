namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The basic solid lever block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicLeverSolidOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicLeverSolidOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.BASIC_LEVER_SOLID_ON;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => true;
    }
}
