namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence snow A block.
    /// </summary>
    public class BlockSequenceSnowA : BlockDataGroup
    {
        public BlockSequenceSnowA(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_SNOW_A, DataSequence.Instance)
        {
        }
    }
}
