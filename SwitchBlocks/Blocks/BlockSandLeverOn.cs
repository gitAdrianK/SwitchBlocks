namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand lever block, capable of only turning the state on.
    /// </summary>
    public class BlockSandLeverOn : ModBlock
    {
        /// <inheritdoc />
        public BlockSandLeverOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SandLeverOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
