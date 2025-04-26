namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;
    using SwitchBlocks.Util;

    /// <summary>
    /// The group snow B block.
    /// </summary>
    public class BlockGroupSnowB : ModBlock, IBlockGroupId
    {
        /// <inheritdoc/>
        public int GroupId { get; set; } = 0;

        /// <inheritdoc/>
        public BlockGroupSnowB(Rectangle collider) : base(collider) { }

        /// <inheritdoc/>
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group)
                    && group.State)
                {
                    return ModBlocks.GROUP_SNOW_B;
                }
                return Color.Transparent;
            }
        }

        /// <inheritdoc/>
        public override bool CanBlockPlayer
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.GroupId, out var group))
                {
                    return group.State;
                }
                return false;
            }
        }
    }
}
