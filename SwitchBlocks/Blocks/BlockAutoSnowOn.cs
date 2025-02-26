namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto snow on block.
    /// </summary>
    public class BlockAutoSnowOn : BlockOn
    {
        public BlockAutoSnowOn(Rectangle collider)
            : base(collider, ModBlocks.AUTO_SNOW_ON, DataAuto.Instance)
        {
        }
    }
}
