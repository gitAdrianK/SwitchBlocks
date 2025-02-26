namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence B block.
    /// </summary>
    public class BlockSequenceB : BlockDataGroup
    {
        public BlockSequenceB(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_B, DataSequence.Instance)
        {
        }
    }
}
