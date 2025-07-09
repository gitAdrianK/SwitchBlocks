namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump ice off block.
    /// </summary>
    public class BlockJumpIceOff : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JumpIceOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
