namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown snow off block.
    /// </summary>
    public class BlockCountdownSnowOff : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataCountdown.Instance.State ? ModBlocks.CountdownSnowOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
