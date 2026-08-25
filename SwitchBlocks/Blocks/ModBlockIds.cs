namespace SwitchBlocks.Blocks
{
    using Grouping;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     A mod block with the additional ability to be grouped by ids.
    /// </summary>
    public abstract class ModBlockIds : ModBlock, IGroupIds
    {
        /// <summary>
        ///     Value representing that a block resets all blocks.
        ///     Note that this is different from not assigned, which is represented as empty array.
        /// </summary>
        public static readonly int[] DefaultMultipleIds = { 0 };

        /// <inheritdoc />
        protected ModBlockIds(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public bool IsSet => this.Value.Length != 0;

        /// <inheritdoc />
        public int[] DefaultValue => DefaultMultipleIds;

        /// <inheritdoc />
        public int[] Value { get; set; }
    }
}
