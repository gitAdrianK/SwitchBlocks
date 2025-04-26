namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto ice on block.
    /// </summary>
    public class BlockAutoIceOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AUTO_ICE_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
