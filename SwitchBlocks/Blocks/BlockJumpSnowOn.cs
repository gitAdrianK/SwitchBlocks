﻿using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;

namespace SwitchBlocks.Blocks
{
    public class BlockJumpSnowOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockJumpSnowOn(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.JUMP_SNOW_ON; }
        }

        public Rectangle GetRect()
        {
            return DataJump.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataJump.State)
                {
                    return BlockCollisionType.Collision_Blocking;
                }
                return BlockCollisionType.Collision_NonBlocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
