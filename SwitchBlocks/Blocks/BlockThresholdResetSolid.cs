namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The threshold solid reset block.
    /// </summary>
    public class BlockThresholdResetSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockThresholdResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.ThresholdResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
