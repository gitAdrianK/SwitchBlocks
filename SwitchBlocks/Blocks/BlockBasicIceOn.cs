namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic ice on block.
    /// </summary>
    public class BlockBasicIceOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicIceOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
