namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto slope off block.
    /// </summary>
    public class BlockAutoSlopeOff : ModSlope
    {
        /// <inheritdoc />
        public BlockAutoSlopeOff(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AutoSlopeOff : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
