using JumpKing.API;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using JumpKing.Workshop;
using Microsoft.Xna.Framework;
using SwitchBlocksMod.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace SwitchBlocksMod.Factories
{
    /// <summary>
    /// Factory for countdown blocks.
    /// </summary>
    public class FactoryCountdown : IBlockFactory
    {
        private static List<Color?> unfilteredColors = new List<Color?> {
            Util.ModBlocks.COUNTDOWN_ON,
            Util.ModBlocks.COUNTDOWN_OFF,
            Util.ModBlocks.COUNTDOWN_LEVER,
            Util.ModBlocks.COUNTDOWN_LEVER_SOLID,
        };
        private static List<Color> filteredColor = unfilteredColors.Where(color => color != null).Select(color => color.Value).ToList();
        private readonly HashSet<Color> supportedBlockCodes = new HashSet<Color>(filteredColor);

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
                case var _ when Util.ModBlocks.COUNTDOWN_ON != null && blockCode == (Color)Util.ModBlocks.COUNTDOWN_ON:
                    return new BlockCountdownOn(blockRect);
                case var _ when Util.ModBlocks.COUNTDOWN_OFF != null && blockCode == (Color)Util.ModBlocks.COUNTDOWN_OFF:
                    return new BlockCountdownOff(blockRect);
                case var _ when Util.ModBlocks.COUNTDOWN_LEVER != null && blockCode == (Color)Util.ModBlocks.COUNTDOWN_LEVER:
                    return new BlockCountdownLever(blockRect);
                case var _ when Util.ModBlocks.COUNTDOWN_LEVER_SOLID != null && blockCode == (Color)Util.ModBlocks.COUNTDOWN_LEVER_SOLID:
                    return new BlockCountdownLeverSolid(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryCountdown).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}

