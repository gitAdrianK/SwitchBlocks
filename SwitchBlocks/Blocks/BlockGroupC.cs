namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group C block.
    /// </summary>
    public class BlockGroupC : BlockDataGroup
    {
        public BlockGroupC(Rectangle collider)
            : base(collider, ModBlocks.GROUP_C, DataGroup.Instance)
        {
        }
    }
}
