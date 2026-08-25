namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown custom duration lever block.
    /// </summary>
    public class BlockCountdownCustomDurationSolid : ModBlockDuration
    {
        /// <inheritdoc />
        public BlockCountdownCustomDurationSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownCustomDurationSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;
    }
}
