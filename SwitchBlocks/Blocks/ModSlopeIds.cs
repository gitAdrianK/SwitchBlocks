namespace SwitchBlocks.Blocks
{
    using Grouping;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A slope block with the additional ability to be grouped by ids.
    /// </summary>
    public abstract class ModSlopeIds : ModSlope, IGroupIds
    {
        /// <inheritdoc />
        protected ModSlopeIds(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public bool IsSet => this.Value.Length != 0;

        /// <inheritdoc />
        public int[] DefaultValue => ModBlockIds.DefaultMultipleIds;

        /// <inheritdoc />
        public int[] Value { get; set; }
    }
}
