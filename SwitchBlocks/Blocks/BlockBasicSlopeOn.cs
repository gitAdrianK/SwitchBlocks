namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic slope on block.
    /// </summary>
    public class BlockBasicSlopeOn : ModSlope
    {
        /// <inheritdoc />
        public BlockBasicSlopeOn(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicSlopeOn : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
