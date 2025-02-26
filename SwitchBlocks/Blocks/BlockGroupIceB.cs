namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group ice B block.
    /// </summary>
    public class BlockGroupIceB : BlockDataGroup
    {
        public BlockGroupIceB(Rectangle collider)
            : base(collider, ModBlocks.GROUP_ICE_B, DataGroup.Instance)
        {
        }
    }
}
