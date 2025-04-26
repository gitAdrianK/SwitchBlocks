namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The jump snow on block.
    /// </summary>
    public class BlockJumpSnowOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockJumpSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JUMP_SNOW_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
