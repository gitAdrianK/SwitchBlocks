namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic slope off block.
    /// </summary>
    public class BlockBasicSlopeOff : ModSlope
    {
        /// <inheritdoc />
        public BlockBasicSlopeOff(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicSlopeOff : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
