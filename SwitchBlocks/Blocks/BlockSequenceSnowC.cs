namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence snow C block.
    /// </summary>
    public class BlockSequenceSnowC : BlockDataGroup
    {
        public BlockSequenceSnowC(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_SNOW_C, DataSequence.Instance)
        {
        }
    }
}
