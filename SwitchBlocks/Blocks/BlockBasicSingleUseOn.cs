namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic single use block, capable of only turning the state on.
    /// </summary>
    public class BlockBasicSingleUseOn : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.GroupId)
            ? ModBlocks.BasicSingleUseOn
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
