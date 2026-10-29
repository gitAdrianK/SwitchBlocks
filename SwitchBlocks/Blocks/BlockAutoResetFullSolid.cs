namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto solid reset full block.
    /// </summary>
    public class BlockAutoResetFullSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoResetFullSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoResetFullSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
