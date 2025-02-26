namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence D block.
    /// </summary>
    public class BlockSequenceD : BlockDataGroup
    {
        public BlockSequenceD(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_D, DataSequence.Instance)
        {
        }
    }
}
