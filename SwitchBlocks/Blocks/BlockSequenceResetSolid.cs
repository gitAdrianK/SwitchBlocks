namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The sequence solid reset block.
    /// </summary>
    public class BlockSequenceResetSolid : ModBlock, IResetGroupIds
    {
        /// <inheritdoc />
        public BlockSequenceResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SequenceResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int[] ResetIDs { get; set; } = { };
    }
}
