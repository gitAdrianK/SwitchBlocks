namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto reset full block.
    /// </summary>
    public class BlockAutoResetFull : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoResetFull(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoResetFull;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
