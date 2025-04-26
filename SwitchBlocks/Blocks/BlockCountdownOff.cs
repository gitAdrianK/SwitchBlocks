namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown off block.
    /// </summary>
    public class BlockCountdownOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataCountdown.Instance.State ? ModBlocks.COUNTDOWN_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
