namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The jump ice on block.
    /// </summary>
    public class BlockJumpIceOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockJumpIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JUMP_ICE_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
