namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold slope off block.
    /// </summary>
    public class BlockThresholdSlopeOff : ModSlope
    {
        /// <inheritdoc />
        public BlockThresholdSlopeOff(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataThreshold.Instance.State ? ModBlocks.ThresholdSlopeOff : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => !DataThreshold.Instance.State;
    }
}
