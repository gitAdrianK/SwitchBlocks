namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group solid reset block.
    /// </summary>
    public class BlockGroupResetSolid : ModBlockIds
    {
        /// <inheritdoc />
        public BlockGroupResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
