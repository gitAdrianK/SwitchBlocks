namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown slope on block.
    /// </summary>
    public class BlockCountdownSlopeOn : ModSlope
    {
        /// <inheritdoc />
        public BlockCountdownSlopeOn(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => DataCountdown.Instance.State ? ModBlocks.CountdownSlopeOn : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
