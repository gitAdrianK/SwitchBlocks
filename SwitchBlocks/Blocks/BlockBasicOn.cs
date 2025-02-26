namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The basic on block.
    /// </summary>
    public class BlockBasicOn : BlockOn
    {
        public BlockBasicOn(Rectangle collider)
            : base(collider, ModBlocks.BASIC_ON, DataBasic.Instance)
        {
        }
    }
}
