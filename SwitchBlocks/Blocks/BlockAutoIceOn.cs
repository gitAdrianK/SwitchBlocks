namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto ice on block.
    /// </summary>
    public class BlockAutoIceOn : BlockOn
    {
        public BlockAutoIceOn(Rectangle collider)
            : base(collider, ModBlocks.AUTO_ICE_ON, DataAuto.Instance)
        {
        }
    }
}
