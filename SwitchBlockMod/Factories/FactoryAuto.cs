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
    /// Factory for auto blocks.
    /// </summary>
    public class FactoryAuto : IBlockFactory
    {
        private static List<Color?> unfilteredColors = new List<Color?> {
            Util.ModBlocks.AUTO_ON,
            Util.ModBlocks.AUTO_OFF,
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
                case var _ when Util.ModBlocks.AUTO_ON != null && blockCode == (Color)Util.ModBlocks.AUTO_ON:
                    return new BlockAutoOn(blockRect);
                case var _ when Util.ModBlocks.AUTO_OFF != null && blockCode == (Color)Util.ModBlocks.AUTO_OFF:
                    return new BlockAutoOff(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryAuto).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
