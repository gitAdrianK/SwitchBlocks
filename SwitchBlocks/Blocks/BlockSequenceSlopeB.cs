namespace SwitchBlocks.Blocks
{
    using Data;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sequence slope B block.
    /// </summary>
    public class BlockSequenceSlopeB : ModSlopeId
    {
        /// <inheritdoc />
        public BlockSequenceSlopeB(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataSequence.Instance.Groups.TryGetValue(this.Value, out var group)
                    && group.State)
                {
                    return ModBlocks.SequenceSlopeB;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        public override bool CanBlockPlayer =>
            DataSequence.Instance.Groups.TryGetValue(this.Value, out var group) && group.State;
    }
}
