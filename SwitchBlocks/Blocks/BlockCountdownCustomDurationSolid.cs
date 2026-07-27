namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown custom duration lever block.
    /// </summary>
    public class BlockCountdownCustomDurationSolid : ModBlock, IBlockDuration
    {
        /// <inheritdoc />
        public BlockCountdownCustomDurationSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownCustomDurationSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int Duration { get; set; } = 0;
    }
}
