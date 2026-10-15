namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic single use block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicSingleUseOn : ModBlockId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.Value)
            ? ModBlocks.BasicSingleUseOn
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
