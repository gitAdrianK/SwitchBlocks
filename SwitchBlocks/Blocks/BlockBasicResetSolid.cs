namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic solid reset block.
    /// </summary>
    public class BlockBasicResetSolid : ModBlockIds
    {
        /// <inheritdoc />
        public BlockBasicResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
