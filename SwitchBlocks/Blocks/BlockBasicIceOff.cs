namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic ice off block.
    /// </summary>
    public class BlockBasicIceOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicIceOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
