namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence snow B block.
    /// </summary>
    public class BlockSequenceSnowB : BlockDataGroup
    {
        public BlockSequenceSnowB(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_SNOW_B, DataSequence.Instance)
        {
        }
    }
}
