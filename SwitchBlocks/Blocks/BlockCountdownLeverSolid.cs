namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The countdown solid lever block.
    /// </summary>
    public class BlockCountdownLeverSolid : ModBlock
    {
        /// <inheritdoc/>
        public BlockCountdownLeverSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.COUNTDOWN_LEVER_SOLID;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => true;
    }
}
