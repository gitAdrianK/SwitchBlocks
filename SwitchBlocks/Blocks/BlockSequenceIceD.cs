namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The sequence ice D block.
    /// </summary>
    public class BlockSequenceIceD : BlockDataGroup
    {
        public BlockSequenceIceD(Rectangle collider)
            : base(collider, ModBlocks.SEQUENCE_ICE_D, DataSequence.Instance)
        {
        }
    }
}
