namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown off block.
    /// </summary>
    public class BlockCountdownOff : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataCountdown.Instance.State ? ModBlocks.CountdownOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
