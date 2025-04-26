namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown snow off block.
    /// </summary>
    public class BlockCountdownSnowOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataCountdown.Instance.State ? ModBlocks.COUNTDOWN_SNOW_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
