namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group D block.
    /// </summary>
    public class BlockGroupD : ModBlockId
    {
        /// <inheritdoc />
        public BlockGroupD(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.Value, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupD;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        protected override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.Value, out var group) && group.State;
    }
}
