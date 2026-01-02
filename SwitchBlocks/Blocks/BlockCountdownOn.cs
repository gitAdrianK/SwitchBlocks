namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown on block.
    /// </summary>
    public class BlockCountdownOn : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataCountdown.Instance.State ? ModBlocks.CountdownOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
