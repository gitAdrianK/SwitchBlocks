﻿using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;

namespace SwitchBlocks.Blocks
{
    /// <summary>
    /// The countdown ice on block.
    /// </summary>
    public class BlockCountdownIceOn : IBlock, IBlockDebugColor
    {
        private readonly Rectangle collider;

        public BlockCountdownIceOn(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.COUNTDOWN_ICE_ON; }
        }

        public Rectangle GetRect()
        {
            return DataCountdown.State ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataCountdown.State)
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
