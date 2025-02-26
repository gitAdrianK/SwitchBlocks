namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence C block.
    /// </summary>
    public class BlockSequenceC : BlockDataGroup
    {
        public BlockSequenceC(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_C, DataSequence.Instance)
        {
        }
    }
}
