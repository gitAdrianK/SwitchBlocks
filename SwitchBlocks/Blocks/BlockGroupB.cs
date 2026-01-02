namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group B block.
    /// </summary>
    public class BlockGroupB : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockGroupB(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupB;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        protected override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group) && group.State;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
