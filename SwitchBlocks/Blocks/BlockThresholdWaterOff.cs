namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold water off block.
    /// </summary>
    public class BlockThresholdWaterOff : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdWaterOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataThreshold.Instance.State ? ModBlocks.ThresholdWaterOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
