namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand on block.
    /// </summary>
    public class BlockSandOn : ModBlock
    {
        /// <inheritdoc />
        public BlockSandOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SandOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
