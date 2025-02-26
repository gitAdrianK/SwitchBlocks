namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group D block.
    /// </summary>
    public class BlockGroupD : BlockDataGroup
    {
        public BlockGroupD(Rectangle collider)
            : base(collider, ModBlocks.GROUP_D, DataGroup.Instance)
        {
        }
    }
}
