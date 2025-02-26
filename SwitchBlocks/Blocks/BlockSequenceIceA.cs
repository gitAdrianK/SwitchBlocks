namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence ice A block.
    /// </summary>
    public class BlockSequenceIceA : BlockDataGroup
    {
        public BlockSequenceIceA(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_ICE_A, DataSequence.Instance)
        {
        }
    }
}
