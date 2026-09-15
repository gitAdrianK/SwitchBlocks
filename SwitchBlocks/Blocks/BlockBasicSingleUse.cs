namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic single use block.
    /// </summary>
    public class BlockBasicSingleUse : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockBasicSingleUse(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.Touched.Contains(this.GroupId)
            ? ModBlocks.BasicSingleUse
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
