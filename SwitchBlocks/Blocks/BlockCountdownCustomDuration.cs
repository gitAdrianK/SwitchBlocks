namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown custom duration lever block.
    /// </summary>
    public class BlockCountdownCustomDuration : ModBlock, IBlockDuration
    {
        /// <inheritdoc />
        public BlockCountdownCustomDuration(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownCustomDuration;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int Duration { get; set; } = 0;
    }
}
