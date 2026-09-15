namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The basic solid reset block.
    /// </summary>
    public class BlockBasicResetSolid : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockBasicResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.BasicResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
