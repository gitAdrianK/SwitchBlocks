namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence ice C block.
    /// </summary>
    public class BlockSequenceIceC : BlockDataGroup
    {
        public BlockSequenceIceC(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_ICE_C, DataSequence.Instance)
        {
        }
    }
}
