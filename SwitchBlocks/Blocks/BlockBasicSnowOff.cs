namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic snow off block.
    /// </summary>
    public class BlockBasicSnowOff : BlockOff
    {
        public BlockBasicSnowOff(Rectangle collider)
            : base(collider, ModBlocks.BASIC_SNOW_OFF, DataBasic.Instance)
        {
        }
    }
}
