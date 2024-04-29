﻿using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Data;

namespace SwitchBlocksMod.Blocks
{
    public class BlockJumpOff : IBlock
    {
        private readonly Rectangle collider;

        public BlockJumpOff(Rectangle collider)
        {
            this.collider = collider;
        }
        public Rectangle GetRect()
        {
            return !DataJump.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return !DataJump.State;
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataJump.State)
                {
                    return BlockCollisionType.Collision_NonBlocking;
                }
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
