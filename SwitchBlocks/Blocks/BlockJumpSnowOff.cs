namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump snow off block.
    /// </summary>
    public class BlockJumpSnowOff : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JumpSnowOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataJump.Instance.State;
    }
}
