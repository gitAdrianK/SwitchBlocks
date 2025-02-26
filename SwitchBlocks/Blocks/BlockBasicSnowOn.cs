namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic snow on block.
    /// </summary>
    public class BlockBasicSnowOn : BlockOn
    {
        public BlockBasicSnowOn(Rectangle collider)
            : base(collider, ModBlocks.BASIC_SNOW_ON, DataBasic.Instance)
        {
        }
    }
}
