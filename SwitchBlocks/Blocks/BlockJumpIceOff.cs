namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The jump ice off block.
    /// </summary>
    public class BlockJumpIceOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockJumpIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JUMP_ICE_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
