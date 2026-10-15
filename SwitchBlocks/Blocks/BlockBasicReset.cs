namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic reset block.
    /// </summary>
    public class BlockBasicReset : ModBlockIds
    {
        /// <inheritdoc />
        public BlockBasicReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
