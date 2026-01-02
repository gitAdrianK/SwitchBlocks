namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump infinity jump on block.
    /// </summary>
    public class BlockJumpInfinityJumpOn : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpInfinityJumpOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JumpInfinityJumpOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
