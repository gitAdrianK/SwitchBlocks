namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow on block.
    /// </summary>
    public class BlockAutoSnowOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AUTO_SNOW_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
