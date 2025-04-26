namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown ice off block.
    /// </summary>
    public class BlockCountdownIceOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataCountdown.Instance.State ? ModBlocks.COUNTDOWN_ICE_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
