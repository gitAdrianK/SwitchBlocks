namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic sand on block.
    /// </summary>
    public class BlockBasicSandOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicSandOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicSandOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
