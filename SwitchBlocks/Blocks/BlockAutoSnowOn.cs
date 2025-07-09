namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto snow on block.
    /// </summary>
    public class BlockAutoSnowOn : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AutoSnowOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
