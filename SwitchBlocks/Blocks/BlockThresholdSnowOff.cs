namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold snow off block.
    /// </summary>
    public class BlockThresholdSnowOff : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataThreshold.Instance.State ? ModBlocks.ThresholdSnowOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataThreshold.Instance.State;
    }
}
