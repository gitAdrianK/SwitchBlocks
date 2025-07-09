namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic solid lever block.
    /// </summary>
    public class BlockBasicLeverSolid : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicLeverSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicLeverSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
