namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpSnowOn : BlockOn
    {
        public BlockJumpSnowOn(Rectangle collider)
            : base(collider, ModBlocks.JUMP_SNOW_ON, DataJump.Instance)
        {
        }
    }
}
