namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Util;

    /// <summary>
    /// The countdown single use lever block.
    /// </summary>
    public class BlockCountdownSingleUse : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockCountdownSingleUse(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_SINGLE_USE;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
