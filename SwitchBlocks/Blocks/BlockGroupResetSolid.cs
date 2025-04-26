namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    /// <summary>
    /// The group solid reset block.
    /// </summary>
    public class BlockGroupResetSolid : ModBlock, IResetGroupIds
    {
        /// <inheritdoc/>
        public int[] ResetIds { get; set; } = { };

        /// <inheritdoc/>
        public BlockGroupResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.GROUP_RESET_SOLID;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => true;
    }
}
