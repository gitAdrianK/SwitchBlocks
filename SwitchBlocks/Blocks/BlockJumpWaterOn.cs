namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump water on block.
    /// </summary>
    public class BlockJumpWaterOn : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpWaterOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataJump.Instance.State ? ModBlocks.JumpWaterOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
