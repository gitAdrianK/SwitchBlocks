namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto ice off block.
    /// </summary>
    public class BlockAutoIceOff : BlockOff
    {
        public BlockAutoIceOff(Rectangle collider)
            : base(collider, ModBlocks.AUTO_ICE_OFF, DataAuto.Instance)
        {
        }
    }
}
