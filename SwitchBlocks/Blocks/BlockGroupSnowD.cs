namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group snow D block.
    /// </summary>
    public class BlockGroupSnowD : BlockDataGroup
    {
        public BlockGroupSnowD(Rectangle collider)
            : base(collider, ModBlocks.GROUP_SNOW_D, DataGroup.Instance)
        {
        }
    }
}
