namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump slope on block.
    /// </summary>
    public class BlockJumpSlopeOn : ModSlope
    {
        /// <inheritdoc />
        public BlockJumpSlopeOn(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JumpSlopeOn : Color.DimGray;

        /// <inheritdoc />
        public override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
