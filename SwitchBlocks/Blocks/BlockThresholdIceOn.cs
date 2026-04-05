namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold ice on block.
    /// </summary>
    public class BlockThresholdIceOn : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataThreshold.Instance.State ? ModBlocks.ThresholdIceOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataThreshold.Instance.State;
    }
}
