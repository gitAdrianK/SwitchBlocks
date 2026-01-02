namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic water off block.
    /// </summary>
    public class BlockBasicWaterOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicWaterOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicWaterOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
