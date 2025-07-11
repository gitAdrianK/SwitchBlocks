namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The sequence reset block.
    /// </summary>
    public class BlockSequenceReset : ModBlock, IResetGroupIds
    {
        /// <inheritdoc />
        public BlockSequenceReset(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.SequenceReset;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int[] ResetIDs { get; set; } = { };
    }
}
