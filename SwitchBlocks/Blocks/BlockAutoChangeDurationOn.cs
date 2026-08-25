namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto change duration on block.
    /// </summary>
    public class BlockAutoChangeDurationOn : ModBlockDuration
    {
        /// <inheritdoc />
        public BlockAutoChangeDurationOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoChangeDurationOn;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
