namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown snow on block.
    /// </summary>
    public class BlockCountdownSnowOn : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownSnowOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor =>
            DataCountdown.Instance.State ? ModBlocks.CountdownSnowOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => DataCountdown.Instance.State;
    }
}
