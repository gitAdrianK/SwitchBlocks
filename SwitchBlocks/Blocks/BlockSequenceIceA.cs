namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The sequence ice A block.
    /// </summary>
    public class BlockSequenceIceA : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockSequenceIceA(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataSequence.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.SequenceIceA;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        protected override bool CanBlockPlayer =>
            DataSequence.Instance.Groups.TryGetValue(this.GroupId, out var group) && group.State;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
