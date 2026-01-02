namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump on block.
    /// </summary>
    public class BlockJumpOn : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JumpOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataJump.Instance.State;
    }
}
