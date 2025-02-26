namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group A block.
    /// </summary>
    public class BlockGroupA : BlockDataGroup
    {
        public BlockGroupA(Rectangle collider)
            : base(collider, ModBlocks.GROUP_A, DataGroup.Instance)
        {
        }
    }
}
