namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand solid lever block, capable of only turning the state off.
    /// </summary>
    public class BlockSandLeverSolidOff : ModBlock
    {
        /// <inheritdoc />
        public BlockSandLeverSolidOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SandLeverSolidOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
