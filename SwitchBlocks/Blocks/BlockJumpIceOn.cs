namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpIceOn : BlockOn
    {
        public BlockJumpIceOn(Rectangle collider)
            : base(collider, ModBlocks.JUMP_ICE_ON, DataJump.Instance)
        {
        }
    }
}
