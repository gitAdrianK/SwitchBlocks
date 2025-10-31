namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic water on block.
    /// </summary>
    public class BlockBasicWaterOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicWaterOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicWaterOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
