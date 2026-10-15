namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown solid single use lever block.
    /// </summary>
    public class BlockCountdownSingleUseSolid : ModBlockId
    {
        /// <inheritdoc />
        public BlockCountdownSingleUseSolid(Rectangle collider) : base(collider) { }

        public override Color DebugColor => !DataCountdown.Instance.Touched.Contains(this.Value)
            ? ModBlocks.CountdownSingleUseSolid
            : Color.DimGray;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
