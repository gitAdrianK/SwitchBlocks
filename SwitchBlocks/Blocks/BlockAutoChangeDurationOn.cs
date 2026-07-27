namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The auto change duration on block.
    /// </summary>
    public class BlockAutoChangeDurationOn : ModBlock, IBlockDuration
    {
        /// <inheritdoc />
        public BlockAutoChangeDurationOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoChangeDurationOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int Duration { get; set; }
    }
}
