namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown single use lever block.
    /// </summary>
    public class BlockCountdownSingleUse : ModBlockId
    {
        /// <inheritdoc />
        public BlockCountdownSingleUse(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataCountdown.Instance.Touched.Contains(this.Value)
            ? ModBlocks.CountdownSingleUse
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
