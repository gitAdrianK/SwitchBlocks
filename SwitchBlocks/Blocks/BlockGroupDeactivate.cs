namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group deactivate block.
    /// </summary>
    public class BlockGroupDeactivate : ModBlockIds
    {
        /// <inheritdoc />
        public BlockGroupDeactivate(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupDeactivate;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
