namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The auto snow off block.
    /// </summary>
    public class BlockAutoSnowOff : ModBlock
    {
        /// <inheritdoc />
        public BlockAutoSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataAuto.Instance.State ? ModBlocks.AutoSnowOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataAuto.Instance.State;
    }
}
