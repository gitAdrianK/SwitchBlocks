namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group snow B block.
    /// </summary>
    public class BlockGroupSnowB : BlockDataGroup
    {
        public BlockGroupSnowB(Rectangle collider)
            : base(collider, ModBlocks.GROUP_SNOW_B, DataGroup.Instance)
        {
        }
    }
}
