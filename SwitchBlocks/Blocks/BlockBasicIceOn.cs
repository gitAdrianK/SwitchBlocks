namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic ice on block.
    /// </summary>
    public class BlockBasicIceOn : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BASIC_ICE_ON : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
