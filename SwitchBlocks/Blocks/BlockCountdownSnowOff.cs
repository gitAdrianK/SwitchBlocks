namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown snow off block.
    /// </summary>
    public class BlockCountdownSnowOff : BlockOff
    {
        public BlockCountdownSnowOff(Rectangle collider)
            : base(collider, ModBlocks.COUNTDOWN_SNOW_OFF, DataCountdown.Instance)
        {
        }
    }
}
