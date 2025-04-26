namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic off block.
    /// </summary>
    public class BlockBasicOff : ModBlock
    {
        /// <inheritdoc/>
        public BlockBasicOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BASIC_OFF : Color.Transparent;

        /// <inheritdoc/>
        public override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
