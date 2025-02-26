namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence snow D block.
    /// </summary>
    public class BlockSequenceSnowD : BlockDataGroup
    {
        public BlockSequenceSnowD(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_SNOW_D, DataSequence.Instance)
        {
        }
    }
}
