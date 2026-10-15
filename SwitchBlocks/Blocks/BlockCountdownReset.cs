namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown reset block.
    /// </summary>
    public class BlockCountdownReset : ModBlockIds
    {
        /// <inheritdoc />
        public BlockCountdownReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
