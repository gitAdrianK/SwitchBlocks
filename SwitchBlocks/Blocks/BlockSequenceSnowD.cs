﻿using JumpKing.Level;
using Microsoft.Xna.Framework;
using SwitchBlocks.Data;
using SwitchBlocks.Util;

namespace SwitchBlocks.Blocks
{
    /// <summary>
    /// The sequence snow D block.
    /// </summary>
    public class BlockSequenceSnowD : IBlock, IBlockDebugColor, IBlockGroupId
    {
        public int GroupId { get; set; } = 0;

        private readonly Rectangle collider;

        public BlockSequenceSnowD(Rectangle collider)
        {
            this.collider = collider;
        }

        public Color DebugColor
        {
            get { return ModBlocks.SEQUENCE_SNOW_D; }
        }

        public Rectangle GetRect()
        {
            return DataSequence.GetState(GroupId) ? collider : new Rectangle(0, 0, 0, 0);
        }

        public BlockCollisionType Intersects(Rectangle hitbox, out Rectangle intersection)
        {
            if (collider.Intersects(hitbox))
            {
                intersection = Rectangle.Intersect(hitbox, collider);
                if (DataSequence.GetState(GroupId))
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