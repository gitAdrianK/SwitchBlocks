namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto solid reset block.
    /// </summary>
    public class BlockAutoResetSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
