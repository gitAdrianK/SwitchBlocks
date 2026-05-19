namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group solid deactivate block.
    /// </summary>
    public class BlockGroupDeactivateSolid : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockGroupDeactivateSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupDeactivateSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
