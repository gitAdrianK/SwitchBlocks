namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group ice C block.
    /// </summary>
    public class BlockGroupIceC : BlockDataGroup
    {
        public BlockGroupIceC(Rectangle collider)
            : base(collider, ModBlocks.GROUP_ICE_C, DataGroup.Instance)
        {
        }
    }
}
