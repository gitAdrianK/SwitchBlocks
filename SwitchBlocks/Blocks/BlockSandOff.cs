namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand off block.
    /// </summary>
    public class BlockSandOff : ModBlock
    {
        /// <inheritdoc />
        public BlockSandOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SandOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
