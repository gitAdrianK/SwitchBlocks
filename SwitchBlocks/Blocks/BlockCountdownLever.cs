namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The countdown lever block.
    /// </summary>
    public class BlockCountdownLever : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownLever(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_LEVER;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => false;
    }
}
