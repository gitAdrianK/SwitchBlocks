namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown ice off block.
    /// </summary>
    public class BlockCountdownIceOff : BlockOff
    {
        public BlockCountdownIceOff(Rectangle collider)
            : base(collider, ModBlocks.COUNTDOWN_ICE_OFF, DataCountdown.Instance)
        {
        }
    }
}
