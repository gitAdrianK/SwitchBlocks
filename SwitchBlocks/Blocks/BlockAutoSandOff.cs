namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto sand off block.
    /// </summary>
    public class BlockAutoSandOff : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoSandOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AutoSandOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
