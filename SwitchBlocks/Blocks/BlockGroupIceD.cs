namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group ice D block.
    /// </summary>
    public class BlockGroupIceD : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockGroupIceD(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupIceD;
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
