namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic solid single use block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicSingleUseSolidOff : ModBlockId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseSolidOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.Value)
            ? ModBlocks.BasicSingleUseSolidOff
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
