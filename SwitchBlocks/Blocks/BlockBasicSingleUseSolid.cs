namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic solid single use block.
    /// </summary>
    public class BlockBasicSingleUseSolid : ModBlockId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.Value)
            ? ModBlocks.BasicSingleUseSolid
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
