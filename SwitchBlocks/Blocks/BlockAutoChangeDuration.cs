namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto change duration block.
    /// </summary>
    public class BlockAutoChangeDuration : ModBlockDuration
    {
        /// <inheritdoc />
        public BlockAutoChangeDuration(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoChangeDuration;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
