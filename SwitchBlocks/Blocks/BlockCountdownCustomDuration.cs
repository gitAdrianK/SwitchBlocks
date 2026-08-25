namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown custom duration lever block.
    /// </summary>
    public class BlockCountdownCustomDuration : ModBlockDuration
    {
        /// <inheritdoc />
        public BlockCountdownCustomDuration(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownCustomDuration;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
