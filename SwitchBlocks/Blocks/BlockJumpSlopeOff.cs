namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump slope off block.
    /// </summary>
    public class BlockJumpSlopeOff : ModSlope
    {
        /// <inheritdoc />
        public BlockJumpSlopeOff(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JumpSlopeOff : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
