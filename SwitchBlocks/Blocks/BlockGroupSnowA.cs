namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group snow A block.
    /// </summary>
    public class BlockGroupSnowA : BlockDataGroup
    {
        public BlockGroupSnowA(Rectangle collider)
            : base(collider, ModBlocks.GROUP_SNOW_A, DataGroup.Instance)
        {
        }
    }
}
