namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown on block.
    /// </summary>
    public class BlockCountdownOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataCountdown.Instance.State ? ModBlocks.COUNTDOWN_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
