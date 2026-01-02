namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto water on block.
    /// </summary>
    public class BlockAutoWaterOn : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoWaterOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AutoWaterOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
