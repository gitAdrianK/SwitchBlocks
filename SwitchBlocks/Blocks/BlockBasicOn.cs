namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic on block.
    /// </summary>
    public class BlockBasicOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataBasic.Instance.State ? ModBlocks.BasicOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataBasic.Instance.State;
    }
}
