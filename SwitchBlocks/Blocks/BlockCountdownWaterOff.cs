namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown water off block.
    /// </summary>
    public class BlockCountdownWaterOff : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownWaterOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataCountdown.Instance.State ? ModBlocks.CountdownWaterOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
