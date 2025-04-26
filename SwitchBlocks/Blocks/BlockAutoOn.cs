namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto on block.
    /// </summary>
    public class BlockAutoOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockAutoOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AUTO_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
