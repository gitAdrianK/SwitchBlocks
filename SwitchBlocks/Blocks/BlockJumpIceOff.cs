namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpIceOff : BlockOff
    {
        public BlockJumpIceOff(Rectangle collider)
            : base(collider, ModBlocks.JUMP_ICE_OFF, DataJump.Instance)
        {
        }
    }
}
