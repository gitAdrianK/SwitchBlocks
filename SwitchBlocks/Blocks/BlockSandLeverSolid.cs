namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    /// The sand solid lever block.
    /// </summary>
    public class BlockSandLeverSolid : ModBlock
    {
        /// <inheritdoc/>
        public BlockSandLeverSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => ModBlocks.SAND_LEVER_SOLID;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => true;
    }
}
