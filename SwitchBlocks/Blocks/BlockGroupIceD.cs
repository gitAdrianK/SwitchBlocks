namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group ice D block.
    /// </summary>
    public class BlockGroupIceD : BlockDataGroup
    {
        public BlockGroupIceD(Rectangle collider)
            : base(collider, ModBlocks.GROUP_ICE_D, DataGroup.Instance)
        {
        }
    }
}
