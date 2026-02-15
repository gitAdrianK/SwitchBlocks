namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown sand on block.
    /// </summary>
    public class BlockCountdownSandOn : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownSandOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            DataCountdown.Instance.State ? ModBlocks.CountdownSandOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
