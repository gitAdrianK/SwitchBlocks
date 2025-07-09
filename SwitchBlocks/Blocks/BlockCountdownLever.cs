namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown lever block.
    /// </summary>
    public class BlockCountdownLever : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownLever(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownLever;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
