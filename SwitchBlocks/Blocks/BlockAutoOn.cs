namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto on block.
    /// </summary>
    public class BlockAutoOn : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataAuto.Instance.State ? ModBlocks.AutoOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataAuto.Instance.State;
    }
}
