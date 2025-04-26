namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic ice off block.
    /// </summary>
    public class BlockBasicIceOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BASIC_ICE_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
