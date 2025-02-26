namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The group snow C block.
    /// </summary>
    public class BlockGroupSnowC : BlockDataGroup
    {
        public BlockGroupSnowC(Rectangle collider)
            : base(collider, ModBlocks.GROUP_SNOW_C, DataGroup.Instance)
        {
        }
    }
}
