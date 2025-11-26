namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic infinity jump off block.
    /// </summary>
    public class BlockBasicInfinityJumpOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicInfinityJumpOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataBasic.Instance.State ? ModBlocks.BasicInfinityJumpOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
