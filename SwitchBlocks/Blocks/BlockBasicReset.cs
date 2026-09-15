namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic reset block.
    /// </summary>
    public class BlockBasicReset : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockBasicReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
