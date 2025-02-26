namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    /// <summary>
    /// The countdown snow on block.
    /// </summary>
    public class BlockCountdownSnowOn : BlockOn
    {
        public BlockCountdownSnowOn(Rectangle collider)
            : base(collider, ModBlocks.COUNTDOWN_SNOW_ON, DataCountdown.Instance)
        {
        }
    }
}
