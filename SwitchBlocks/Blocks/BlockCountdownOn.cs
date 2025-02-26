namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown on block.
    /// </summary>
    public class BlockCountdownOn : BlockOn
    {
        public BlockCountdownOn(Rectangle collider)
            : base(collider, ModBlocks.COUNTDOWN_ON, DataCountdown.Instance)
        {
        }
    }
}
