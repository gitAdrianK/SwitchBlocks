namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand solid lever block, capable of only turning the state on.
    /// </summary>
    public class BlockSandLeverSolidOn : ModBlock
    {
        /// <inheritdoc />
        public BlockSandLeverSolidOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SandLeverSolidOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
