namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold water on block.
    /// </summary>
    public class BlockThresholdWaterOn : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdWaterOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataThreshold.Instance.State ? ModBlocks.ThresholdWaterOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
