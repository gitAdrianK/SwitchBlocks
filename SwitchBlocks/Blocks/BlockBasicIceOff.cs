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
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicIceOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
