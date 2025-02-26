namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence A block.
    /// </summary>
    public class BlockSequenceA : BlockDataGroup
    {
        public BlockSequenceA(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_A, DataSequence.Instance)
        {
        }
    }
}
