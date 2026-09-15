namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic solid single use block.
    /// </summary>
    public class BlockBasicSingleUseSolid : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockBasicSingleUseSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.GroupId)
            ? ModBlocks.BasicSingleUseSolid
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
