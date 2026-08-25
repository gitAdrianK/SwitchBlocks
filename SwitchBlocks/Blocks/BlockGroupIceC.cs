namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group ice C block.
    /// </summary>
    public class BlockGroupIceC : ModBlockId
    {
        /// <inheritdoc />
        public BlockGroupIceC(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.Value, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupIceC;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        protected override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.Value, out var group) && group.State;
    }
}
