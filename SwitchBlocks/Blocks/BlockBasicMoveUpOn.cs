namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic move up on block.
    /// </summary>
    public class BlockBasicMoveUpOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicMoveUpOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicMoveUpOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
