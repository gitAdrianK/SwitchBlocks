namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump infinity jump off block.
    /// </summary>
    public class BlockJumpInfinityJumpOff : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpInfinityJumpOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JumpInfinityJumpOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
