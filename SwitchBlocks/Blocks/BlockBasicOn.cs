namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic on block.
    /// </summary>
    public class BlockBasicOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BASIC_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
