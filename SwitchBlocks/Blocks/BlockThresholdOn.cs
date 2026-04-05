namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold on block.
    /// </summary>
    public class BlockThresholdOn : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataThreshold.Instance.State ? ModBlocks.ThresholdOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataThreshold.Instance.State;
    }
}
