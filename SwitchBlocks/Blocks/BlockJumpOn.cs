namespace SwitchBlocks.Blocks
{
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Data;

    public class BlockJumpOn : BlockOn
    {
        public BlockJumpOn(Rectangle collider)
            : base(collider, ModBlocks.JUMP_ON, DataJump.Instance)
        {
        }
    }
}
