namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto off block.
    /// </summary>
    public class BlockAutoOff : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AutoOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
