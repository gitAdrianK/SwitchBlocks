namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic solid single use block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicSingleUseSolidOff : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseSolidOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.GroupId)
            ? ModBlocks.BasicSingleUseSolidOff
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
