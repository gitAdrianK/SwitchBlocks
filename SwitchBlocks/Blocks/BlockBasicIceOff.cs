namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic ice off block.
    /// </summary>
    public class BlockBasicIceOff : BlockOff
    {
        public BlockBasicIceOff(Rectangle collider)
            : base(collider, ModBlocks.BASIC_ICE_OFF, DataBasic.Instance)
        {
        }
    }
}
