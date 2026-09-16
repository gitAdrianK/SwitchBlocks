namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown reset block.
    /// </summary>
    public class BlockCountdownReset : ModBlock, IMultipleGroupIds
    {
        /// <inheritdoc />
        public BlockCountdownReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] Ids { get; set; } = { };
    }
}
