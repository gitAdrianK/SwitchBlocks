namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold ice off block.
    /// </summary>
    public class BlockThresholdIceOff : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataThreshold.Instance.State ? ModBlocks.ThresholdIceOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataThreshold.Instance.State;
    }
}
