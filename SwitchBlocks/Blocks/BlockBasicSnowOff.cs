namespace SwitchBlocks.Blocks
{
    using Data;
    using Microsoft.Xna.Framework;

    /// <summary>
    ///     The basic snow off block.
    /// </summary>
    public class BlockBasicSnowOff : ModBlock
    {
        /// <inheritdoc />
        public BlockBasicSnowOff(Rectangle collider) : base(collider) { }

        /// <inheritdoc />
        public override Color DebugColor => !DataBasic.Instance.State ? ModBlocks.BasicSnowOff : Color.Transparent;

        /// <inheritdoc />
        protected override bool CanBlockPlayer => !DataBasic.Instance.State;
    }
}
