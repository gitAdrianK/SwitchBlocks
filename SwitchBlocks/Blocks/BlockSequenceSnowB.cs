namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// The sequence snow B block.
    /// </summary>
    public class BlockSequenceSnowB : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockSequenceSnowB(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor
        {
            get
            {
                if (DataSequence.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.SEQUENCE_SNOW_B;
                }
                return Color.Transparent;
            }
        }

        /// <inheritdoc/>
        public override bool CanBlockPlayer
        {
            get
            {
                if (DataSequence.Instance.Groups.TryGetValue(this.GroupId, out var group))
                {
                    return group.State;
                }
                return false;
            }
        }
    }
}
