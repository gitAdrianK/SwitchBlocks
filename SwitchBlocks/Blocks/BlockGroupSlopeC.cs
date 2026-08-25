namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group slope C block.
    /// </summary>
    public class BlockGroupSlopeC : ModSlopeId
    {
        /// <inheritdoc />
        public BlockGroupSlopeC(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.Value, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupSlopeC;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        public override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.Value, out var group) && group.State;
    }
}
