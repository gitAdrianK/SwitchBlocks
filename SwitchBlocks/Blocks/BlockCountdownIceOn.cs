namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown ice on block.
    /// </summary>
    public class BlockCountdownIceOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataCountdown.Instance.State ? ModBlocks.COUNTDOWN_ICE_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
