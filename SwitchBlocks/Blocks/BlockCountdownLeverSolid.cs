namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown solid lever block.
    /// </summary>
    public class BlockCountdownLeverSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownLeverSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownLeverSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
