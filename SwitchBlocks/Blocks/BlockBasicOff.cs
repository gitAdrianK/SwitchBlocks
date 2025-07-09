namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic off block.
    /// </summary>
    public class BlockBasicOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
