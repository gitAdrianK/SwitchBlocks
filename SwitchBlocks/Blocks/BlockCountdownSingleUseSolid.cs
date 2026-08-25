namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown solid single use lever block.
    /// </summary>
    public class BlockCountdownSingleUseSolid : ModBlockId
    {
        /// <inheritdoc />
        public BlockCountdownSingleUseSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownSingleUseSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
