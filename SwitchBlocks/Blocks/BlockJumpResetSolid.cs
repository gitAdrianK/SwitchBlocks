namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump solid reset block.
    /// </summary>
    public class BlockJumpResetSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.JumpResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
