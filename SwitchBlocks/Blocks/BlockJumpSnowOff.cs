namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The jump snow off block.
    /// </summary>
    public class BlockJumpSnowOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockJumpSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JUMP_SNOW_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
