namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group B block.
    /// </summary>
    public class BlockGroupB : BlockDataGroup
    {
        public BlockGroupB(Rectangle collider)
            : base(collider, ModBlocks.GROUP_B, DataGroup.Instance)
        {
        }
    }
}
