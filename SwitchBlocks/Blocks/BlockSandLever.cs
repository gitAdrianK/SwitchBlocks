namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand lever block.
    /// </summary>
    public class BlockSandLever : ModBlock
    {
        /// <inheritdoc />
        public BlockSandLever(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SandLever;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
