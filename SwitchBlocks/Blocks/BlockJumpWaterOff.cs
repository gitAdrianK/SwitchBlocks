namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The jump water off block.
    /// </summary>
    public class BlockJumpWaterOff : ModBlock
    {
        /// <inheritdoc />
        public BlockJumpWaterOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataJump.Instance.State ? ModBlocks.JumpWaterOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
