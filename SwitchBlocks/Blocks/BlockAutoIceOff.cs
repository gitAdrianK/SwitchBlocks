namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto ice off block.
    /// </summary>
    public class BlockAutoIceOff : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoIceOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AutoIceOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
