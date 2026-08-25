namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group slope B block.
    /// </summary>
    public class BlockGroupSlopeB : ModSlopeId
    {
        /// <inheritdoc />
        public BlockGroupSlopeB(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.Value, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupSlopeB;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        public override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.Value, out var group) && group.State;
    }
}
