﻿using JumpKing.API;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using JumpKing.Workshop;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Blocks;
using System;
using System.Collections.Generic;

namespace SwitchBlocksMod.Factories
{
    /// <summary>
    /// Factory for sand blocks.
    /// </summary>
    public class FactorySand : IBlockFactory
    {
        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            Util.ModBlocks.SAND_ON,
            Util.ModBlocks.SAND_OFF,
            Util.ModBlocks.SAND_LEVER,
            Util.ModBlocks.SAND_LEVER_ON,
            Util.ModBlocks.SAND_LEVER_OFF,
            Util.ModBlocks.SAND_LEVER_SOLID,
            Util.ModBlocks.SAND_LEVER_SOLID_ON,
            Util.ModBlocks.SAND_LEVER_SOLID_OFF,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            switch (blockCode)
            {
                case var _ when blockCode == Util.ModBlocks.SAND_ON:
                    return new BlockSandOn(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_OFF:
                    return new BlockSandOff(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_LEVER:
                    return new BlockSandLever(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_LEVER_ON:
                    return new BlockSandLeverOn(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_LEVER_OFF:
                    return new BlockSandLeverOff(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_LEVER_SOLID:
                    return new BlockSandLeverSolid(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_LEVER_SOLID_ON:
                    return new BlockSandLeverSolidOn(blockRect);
                case var _ when blockCode == Util.ModBlocks.SAND_LEVER_SOLID_OFF:
                    return new BlockSandLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactorySand).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
