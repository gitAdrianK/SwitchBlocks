namespace SwitchBlocks.Blocks
{
    using Grouping;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A slope block with the additional ability to be grouped by an id.
    /// </summary>
    public abstract class ModSlopeId : ModSlope, IGroupId
    {
        /// <inheritdoc />
        protected ModSlopeId(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public bool IsSet => this.Value != ModBlockId.NotAssigned;

        /// <inheritdoc />
        public int DefaultValue => ModBlockId.NotAssigned;

        /// <inheritdoc />
        public int Value { get; set; }
    }
}
