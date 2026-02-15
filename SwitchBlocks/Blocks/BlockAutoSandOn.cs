namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto Sand on block.
    /// </summary>
    public class BlockAutoSandOn : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoSandOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AutoSandOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
