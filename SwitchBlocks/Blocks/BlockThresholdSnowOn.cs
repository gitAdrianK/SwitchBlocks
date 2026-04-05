namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold snow on block.
    /// </summary>
    public class BlockThresholdSnowOn : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataThreshold.Instance.State ? ModBlocks.ThresholdSnowOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataThreshold.Instance.State;
    }
}
