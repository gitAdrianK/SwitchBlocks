namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic off block.
    /// </summary>
    public class BlockBasicOff : BlockOff
    {
        public BlockBasicOff(Rectangle collider)
            : base(collider, ModBlocks.BASIC_OFF, DataBasic.Instance)
        {
        }
    }
}
