namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand on block.
    /// </summary>
    public class BlockSandOn : ModBlock
    {
        /// <inheritdoc />
        public BlockSandOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            DataSand.Instance.State ? ModBlocks.SandOn : ModBlocks.SandOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
