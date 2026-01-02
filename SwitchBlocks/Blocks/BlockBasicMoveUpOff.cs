namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic move up off block.
    /// </summary>
    public class BlockBasicMoveUpOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicMoveUpOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicMoveUpOff : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
