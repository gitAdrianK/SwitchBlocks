namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow off block.
    /// </summary>
    public class BlockAutoSnowOff : BlockOff
    {
        public BlockAutoSnowOff(Rectangle collider)
            : base(collider, ModBlocks.AUTO_SNOW_OFF, DataAuto.Instance)
        {
        }
    }
}
