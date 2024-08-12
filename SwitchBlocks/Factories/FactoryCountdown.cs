﻿using JumpKing.API;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using JumpKing.Workshop;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using System;
using System.Collections.Generic;

namespace SwitchBlocks.Factories
{
    /// <summary>
    /// Factory for countdown blocks.
    /// </summary>
    public class FactoryCountdown : IBlockFactory
    {
        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            ModBlocks.COUNTDOWN_ON,
            ModBlocks.COUNTDOWN_OFF,
            ModBlocks.COUNTDOWN_LEVER,
            ModBlocks.COUNTDOWN_LEVER_SOLID,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.COUNTDOWN_ON:
                    return true;
                case var _ when blockCode == ModBlocks.COUNTDOWN_OFF:
                    return true;
                case var _ when blockCode == ModBlocks.COUNTDOWN_LEVER_SOLID:
                    return true;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.COUNTDOWN_ON:
                    return new BlockCountdownOn(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_OFF:
                    return new BlockCountdownOff(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_LEVER:
                    return new BlockCountdownLever(blockRect);
                case var _ when blockCode == ModBlocks.COUNTDOWN_LEVER_SOLID:
                    return new BlockCountdownLeverSolid(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryCountdown).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
