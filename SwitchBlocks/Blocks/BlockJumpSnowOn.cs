namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump snow on block.
    /// </summary>
    public class BlockJumpSnowOn : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JumpSnowOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
