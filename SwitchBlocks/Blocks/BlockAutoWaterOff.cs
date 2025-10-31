namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto water off block.
    /// </summary>
    public class BlockAutoWaterOff : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoWaterOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AutoWaterOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
