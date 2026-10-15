namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic single use block.
    /// </summary>
    public class BlockBasicSingleUse : ModBlockId
    {
        /// <inheritdoc />
        public BlockBasicSingleUse(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.Value)
            ? ModBlocks.BasicSingleUse
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
