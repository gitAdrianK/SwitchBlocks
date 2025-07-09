namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto ice on block.
    /// </summary>
    public class BlockAutoIceOn : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AutoIceOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
