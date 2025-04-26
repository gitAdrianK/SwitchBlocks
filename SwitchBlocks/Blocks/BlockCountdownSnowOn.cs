namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown snow on block.
    /// </summary>
    public class BlockCountdownSnowOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataCountdown.Instance.State ? ModBlocks.COUNTDOWN_SNOW_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
