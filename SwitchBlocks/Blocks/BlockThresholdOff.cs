namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold off block.
    /// </summary>
    public class BlockThresholdOff : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataThreshold.Instance.State ? ModBlocks.ThresholdOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataThreshold.Instance.State;
    }
}
