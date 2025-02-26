namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;

    using SwitchBlocks.Data;

    /// <summary>
    /// The auto on block.
    /// </summary>
    public class BlockAutoOn : BlockOn
    {
        public BlockAutoOn(Rectangle collider)
            : base(collider, ModBlocks.AUTO_ON, DataAuto.Instance)
        {
        }
    }
}
