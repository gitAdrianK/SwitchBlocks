namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown single use lever block.
    /// </summary>
    public class BlockCountdownSingleUse : ModBlockId
    {
        /// <inheritdoc />
        public BlockCountdownSingleUse(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownSingleUse;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
