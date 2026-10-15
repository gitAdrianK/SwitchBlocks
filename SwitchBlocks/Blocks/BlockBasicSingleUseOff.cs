namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic single use block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicSingleUseOff : ModBlockId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.Value)
            ? ModBlocks.BasicSingleUseOff
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
