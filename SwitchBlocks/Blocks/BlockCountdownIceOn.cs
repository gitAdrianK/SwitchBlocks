namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown ice on block.
    /// </summary>
    public class BlockCountdownIceOn : BlockOn
    {
        public BlockCountdownIceOn(Rectangle collider)
            : base(collider, ModBlocks.COUNTDOWN_ICE_ON, DataCountdown.Instance)
        {
        }
    }
}
