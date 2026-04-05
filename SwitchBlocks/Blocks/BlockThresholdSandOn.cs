namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold sand on block.
    /// </summary>
    public class BlockThresholdSandOn : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdSandOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataThreshold.Instance.State ? ModBlocks.ThresholdSandOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
