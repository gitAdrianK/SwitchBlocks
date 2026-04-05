namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold slope on block.
    /// </summary>
    public class BlockThresholdSlopeOn : ModSlope
    {
        /// <inheritdoc />
        public BlockThresholdSlopeOn(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => DataThreshold.Instance.State ? ModBlocks.ThresholdSlopeOn : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => DataThreshold.Instance.State;
    }
}
