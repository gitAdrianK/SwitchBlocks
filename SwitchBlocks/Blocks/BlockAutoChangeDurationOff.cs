namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto change duration off block.
    /// </summary>
    public class BlockAutoChangeDurationOff : ModBlockDuration
    {
        /// <inheritdoc />
        public BlockAutoChangeDurationOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.AutoChangeDurationOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
