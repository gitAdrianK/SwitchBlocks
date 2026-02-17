namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group slope D block.
    /// </summary>
    public class BlockGroupSlopeD : ModSlope, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockGroupSlopeD(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupSlopeD;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        public override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group) && group.State;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
