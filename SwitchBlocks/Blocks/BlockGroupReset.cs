namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group reset block.
    /// </summary>
    public class BlockGroupReset : ModBlockIds
    {
        /// <inheritdoc />
        public BlockGroupReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
