namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group ice A block.
    /// </summary>
    public class BlockGroupIceA : BlockDataGroup
    {
        public BlockGroupIceA(Rectangle collider)
            : base(collider, ModBlocks.GROUP_ICE_A, DataGroup.Instance)
        {
        }
    }
}
