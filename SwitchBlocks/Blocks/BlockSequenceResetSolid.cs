namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sequence solid reset block.
    /// </summary>
    public class BlockSequenceResetSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockSequenceResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SequenceResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
