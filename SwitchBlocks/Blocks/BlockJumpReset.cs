namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump reset block.
    /// </summary>
    public class BlockJumpReset : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.JumpReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
