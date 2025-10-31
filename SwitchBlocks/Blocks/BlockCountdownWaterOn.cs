namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown water on block.
    /// </summary>
    public class BlockCountdownWaterOn : ModBlock
    {
        /// <inheritdoc />
        public BlockCountdownWaterOn(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => DataCountdown.Instance.State ? ModBlocks.CountdownWaterOn : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
