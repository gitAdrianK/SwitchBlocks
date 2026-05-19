namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group solid reset block.
    /// </summary>
    public class BlockGroupResetSolid : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockGroupResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
