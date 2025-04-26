namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The jump off block.
    /// </summary>
    public class BlockJumpOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockJumpOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JUMP_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
