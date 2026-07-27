namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The auto change duration block.
    /// </summary>
    public class BlockAutoChangeDuration : ModBlock, IBlockDuration
    {
        /// <inheritdoc />
        public BlockAutoChangeDuration(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoChangeDuration;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int Duration { get; set; }
    }
}
