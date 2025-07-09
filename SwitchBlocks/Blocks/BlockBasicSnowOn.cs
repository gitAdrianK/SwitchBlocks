namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic snow on block.
    /// </summary>
    public class BlockBasicSnowOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicSnowOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
