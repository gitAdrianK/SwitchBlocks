namespace SwitchBlocks.Blocks
{
    using Grouping;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A mod block with the additional ability to be grouped by a duration.
    /// </summary>
    public abstract class ModBlockDuration : ModBlock, IDuration
    {
        /// <summary>Value representing that a block was not assigned a duration.</summary>
        public const int NotAssigned = 0;

        /// <summary>Default duration to assign blocks to when not assigned from seed.</summary>
        private const float DefaultDuration = 3.0f;

        /// <summary>Default duration in ticks.</summary>
        public const int DefaultTicks = (int)((DefaultDuration / ModConstants.DeltaTime) + 0.5f);

        /// <inheritdoc />
        protected ModBlockDuration(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public bool IsSet => this.Value != NotAssigned;

        /// <inheritdoc />
        public int DefaultValue => DefaultTicks;

        /// <inheritdoc />
        public int Value { get; set; }
    }
}
