namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic single use block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicSingleUseOff : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.GroupId)
            ? ModBlocks.BasicSingleUseOff
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
