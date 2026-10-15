namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic solid single use block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicSingleUseSolidOn : ModBlockId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseSolidOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.Value)
            ? ModBlocks.BasicSingleUseSolidOn
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
