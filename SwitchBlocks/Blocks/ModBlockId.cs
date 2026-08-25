namespace SwitchBlocks.Blocks
{
    using Grouping;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A mod block with the additional ability to be grouped by an id.
    /// </summary>
    public abstract class ModBlockId : ModBlock, IGroupId
    {
        /// <summary>Value representing that a block was not assigned an id.</summary>
        public const int NotAssigned = 0;

        /// <inheritdoc />
        protected ModBlockId(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public bool IsSet => this.Value != NotAssigned;

        /// <inheritdoc />
        public int DefaultValue => NotAssigned;

        /// <inheritdoc />
        public int Value { get; set; }
    }
}
