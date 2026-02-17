namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto slope on block.
    /// </summary>
    public class BlockAutoSlopeOn : ModSlope
    {
        /// <inheritdoc />
        public BlockAutoSlopeOn(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AutoSlopeOn : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
