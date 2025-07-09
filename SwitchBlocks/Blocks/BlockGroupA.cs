namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group A block.
    /// </summary>
    public class BlockGroupA : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockGroupA(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupA;
                }

                return Color.Transparent;
            }
        }

        /// <inheritdoc />
        protected override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group) && group.State;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
