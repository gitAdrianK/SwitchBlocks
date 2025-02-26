namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpOff : BlockOff
    {
        public BlockJumpOff(Rectangle collider)
            : base(collider, ModBlocks.JUMP_OFF, DataJump.Instance)
        {
        }
    }
}
