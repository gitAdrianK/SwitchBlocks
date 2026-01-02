namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown ice on block.
    /// </summary>
    public class BlockCountdownIceOn : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownIceOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            DataCountdown.Instance.State ? ModBlocks.CountdownIceOn : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
