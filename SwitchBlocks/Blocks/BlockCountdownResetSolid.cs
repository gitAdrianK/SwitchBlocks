namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The countdown solid reset block.
    /// </summary>
    public class BlockCountdownResetSolid : ModBlockIds
    {
        /// <inheritdoc />
        public BlockCountdownResetSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownResetSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;
    }
}
