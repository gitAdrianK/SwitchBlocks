namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto ice off block.
    /// </summary>
    public class BlockAutoIceOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AUTO_ICE_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
