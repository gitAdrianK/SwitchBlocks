namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto off block.
    /// </summary>
    public class BlockAutoOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AUTO_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
