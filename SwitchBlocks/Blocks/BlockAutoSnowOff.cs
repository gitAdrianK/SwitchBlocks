namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow off block.
    /// </summary>
    public class BlockAutoSnowOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AUTO_SNOW_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
