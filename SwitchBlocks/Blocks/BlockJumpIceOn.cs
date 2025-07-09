namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump ice on block.
    /// </summary>
    public class BlockJumpIceOn : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JumpIceOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
