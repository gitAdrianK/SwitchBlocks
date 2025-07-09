namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic solid lever block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicLeverSolidOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicLeverSolidOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicLeverSolidOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
