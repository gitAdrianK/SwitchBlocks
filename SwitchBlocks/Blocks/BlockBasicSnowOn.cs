namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic snow on block.
    /// </summary>
    public class BlockBasicSnowOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BASIC_SNOW_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
