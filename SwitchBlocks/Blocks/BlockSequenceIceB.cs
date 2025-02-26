namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence ice B block.
    /// </summary>
    public class BlockSequenceIceB : BlockDataGroup
    {
        public BlockSequenceIceB(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_ICE_B, DataSequence.Instance)
        {
        }
    }
}
