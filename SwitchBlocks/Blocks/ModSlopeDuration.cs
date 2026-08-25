namespace SwitchBlocks.Blocks
{
    using Grouping;
    using JumpKing.Level;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A slope block with the additional ability to be grouped by a duration.
    /// </summary>
    public abstract class ModSlopeDuration : ModSlope, IDuration
    {
        /// <inheritdoc />
        protected ModSlopeDuration(Rectangle collider, SlopeType slopeType) : base(collider, slopeType) { }

        /// <inheritdoc />
        public bool IsSet => this.Value != ModBlockDuration.NotAssigned;

        /// <inheritdoc />
        public int DefaultValue => ModBlockDuration.DefaultTicks;

        /// <inheritdoc />
        public int Value { get; set; }
    }
}
