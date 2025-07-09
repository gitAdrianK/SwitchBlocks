namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown ice off block.
    /// </summary>
    public class BlockCountdownIceOff : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataCountdown.Instance.State ? ModBlocks.CountdownIceOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
