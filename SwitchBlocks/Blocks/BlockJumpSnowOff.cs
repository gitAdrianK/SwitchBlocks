namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpSnowOff : BlockOff
    {
        public BlockJumpSnowOff(Rectangle collider)
            : base(collider, ModBlocks.JUMP_SNOW_OFF, DataJump.Instance)
        {
        }
    }
}
