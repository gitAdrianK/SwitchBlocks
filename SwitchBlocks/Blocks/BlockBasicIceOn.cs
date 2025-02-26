namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic ice on block.
    /// </summary>
    public class BlockBasicIceOn : BlockOn
    {
        public BlockBasicIceOn(Rectangle collider)
            : base(collider, ModBlocks.BASIC_ICE_ON, DataBasic.Instance)
        {
        }
    }
}
