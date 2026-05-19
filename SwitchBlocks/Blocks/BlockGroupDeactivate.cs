namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group deactivate block.
    /// </summary>
    public class BlockGroupDeactivate : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockGroupDeactivate(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupDeactivate;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
