namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The group snow C block.
    /// </summary>
    public class BlockGroupSnowC : ModBlockId
    {
        /// <inheritdoc />
        public BlockGroupSnowC(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor
        {
            get
            {
                if (DataGroup.Instance.Groups.TryGetValue(this.Value, out var group)
                    && group.State)
                {
                    return ModBlocks.GroupSnowC;
                }

                return Color.DimGray;
            }
        }

        /// <inheritdoc />
        protected override bool CanBlockPlayer =>
            DataGroup.Instance.Groups.TryGetValue(this.Value, out var group) && group.State;
    }
}
