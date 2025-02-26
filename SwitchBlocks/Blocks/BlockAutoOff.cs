namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The auto off block.
    /// </summary>
    public class BlockAutoOff : BlockOff
    {
        public BlockAutoOff(Rectangle collider)
            : base(collider, ModBlocks.AUTO_OFF, DataAuto.Instance)
        {
        }
    }
}
