﻿using JumpKing.Level;
using Microsoft.Xna.Framework;

namespace SwitchBlocksMod.Blocks
{
    /// <summary>
    /// The basic lever block, capable of only turning the state off.
    /// </summary>
    public class BlockBasicLeverSolidOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicLeverSolidOff(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.BASIC_LEVER_SOLID_OFF; }
        }

        public Rectangle GetRect()
        {
            return collider;
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                return BlockCollisionType.Collision_Blocking;
            }
            intersection = Rectangle.Empty;
            return BlockCollisionType.NoCollision;
        }
    }
}
