namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown single use lever block.
    /// </summary>
    public class BlockCountdownSingleUse : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockCountdownSingleUse(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownSingleUse;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => false;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
