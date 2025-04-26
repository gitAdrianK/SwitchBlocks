namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic snow off block.
    /// </summary>
    public class BlockBasicSnowOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BASIC_SNOW_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
