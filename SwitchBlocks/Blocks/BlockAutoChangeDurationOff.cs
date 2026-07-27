namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The auto change duration off block.
    /// </summary>
    public class BlockAutoChangeDurationOff : ModBlock, IBlockDuration
    {
        /// <inheritdoc />
        public BlockAutoChangeDurationOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoChangeDurationOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int Duration { get; set; }
    }
}
