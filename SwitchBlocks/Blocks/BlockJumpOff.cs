namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump off block.
    /// </summary>
    public class BlockJumpOff : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JumpOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
