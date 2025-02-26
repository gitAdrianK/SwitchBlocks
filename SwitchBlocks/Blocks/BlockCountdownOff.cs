namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown off block.
    /// </summary>
    public class BlockCountdownOff : BlockOff
    {
        public BlockCountdownOff(Rectangle collider)
            : base(collider, ModBlocks.COUNTDOWN_OFF, DataCountdown.Instance)
        {
        }
    }
}
