namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic infinity jump on block.
    /// </summary>
    public class BlockBasicInfinityJumpOn : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicInfinityJumpOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            DataBasic.Instance.State ? ModBlocks.BasicInfinityJumpOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
