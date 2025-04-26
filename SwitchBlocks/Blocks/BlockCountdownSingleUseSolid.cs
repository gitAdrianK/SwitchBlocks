namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    /// <summary>
    /// The countdown solid single use lever block.
    /// </summary>
    public class BlockCountdownSingleUseSolid : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockCountdownSingleUseSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_SINGLE_USE_SOLID;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => true;
    }
}
