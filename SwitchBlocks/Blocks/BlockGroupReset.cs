namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The group reset block.
    /// </summary>
    public class BlockGroupReset : ModBlock, IResetGroupIds
    {
        /// <inheritdoc />
        public BlockGroupReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.GroupReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] ResetIDs { get; set; } = { };
    }
}
