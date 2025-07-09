namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using Util;

    /// <summary>
    ///     The countdown solid single use lever block.
    /// </summary>
    public class BlockCountdownSingleUseSolid : ModBlock, IBlockGroupId
    {
        /// <inheritdoc />
        public BlockCountdownSingleUseSolid(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => ModBlocks.CountdownSingleUseSolid;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => true;

        /// <inheritdoc />
        public int GroupId { get; set; } = 0;
    }
}
