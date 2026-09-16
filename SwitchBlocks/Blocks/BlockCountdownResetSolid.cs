namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown solid reset block.
    /// </summary>
    public class BlockCountdownResetSolid : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockCountdownResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
