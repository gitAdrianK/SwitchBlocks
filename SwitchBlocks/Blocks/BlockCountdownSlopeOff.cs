namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown slope off block.
    /// </summary>
    public class BlockCountdownSlopeOff : ModSlope
    {
        /// <inheritdoc />
        public BlockCountdownSlopeOff(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataCountdown.Instance.State ? ModBlocks.CountdownSlopeOff : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => !DataCountdown.Instance.State;
    }
}
