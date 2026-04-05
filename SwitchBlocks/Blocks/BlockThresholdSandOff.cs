namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold sand off block.
    /// </summary>
    public class BlockThresholdSandOff : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdSandOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataThreshold.Instance.State ? ModBlocks.ThresholdSandOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
