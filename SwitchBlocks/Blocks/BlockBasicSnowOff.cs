﻿using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;

namespace SwitchBlocks.Blocks
{
    /// <summary>
    /// The basic snow off block.
    /// </summary>
    public class BlockBasicSnowOff : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockBasicSnowOff(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.BASIC_SNOW_OFF; }
        }

        public Rectangle GetRect()
        {
            return !DataBasic.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataBasic.State)
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
