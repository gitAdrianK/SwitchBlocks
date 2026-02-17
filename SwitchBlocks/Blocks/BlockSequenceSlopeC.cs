namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The sequence slope C block.
    /// </summary>
    public class BlockSequenceSlopeC : ModSlope, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockSequenceSlopeC(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataSequence.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.SequenceSlopeC;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        public override bool CanBlockPlayer =>
            DataSequence.Instance.Groups.TryGetValue(this.GroupId, out var group) && group.State;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
