namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group solid deactivate block.
    /// </summary>
    public class BlockGroupDeactivateSolid : ModBlockIds
    {
        /// <inheritdoc />
        public BlockGroupDeactivateSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupDeactivateSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
