namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The jump on block.
    /// </summary>
    public class BlockJumpOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockJumpOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JUMP_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
