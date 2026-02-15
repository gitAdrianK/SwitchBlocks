namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown sand off block.
    /// </summary>
    public class BlockCountdownSandOff : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownSandOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataCountdown.Instance.State ? ModBlocks.CountdownSandOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
