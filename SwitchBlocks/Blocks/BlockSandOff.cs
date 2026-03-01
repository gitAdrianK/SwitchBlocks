namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The sand off block.
    /// </summary>
    public class BlockSandOff : ModBlock
    {
        /// <inheritdoc />
        public BlockSandOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            !DataSand.Instance.State ? ModBlocks.SandOn : ModBlocks.SandOff;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
