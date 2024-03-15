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
    /// Factory for basic blocks.
    /// </summary>
    public class FactoryBasic : IBlockFactory
    {
        private static List<Color?> unfilteredColors = new List<Color?> {
            Util.ModBlocks.BASIC_ON,
            Util.ModBlocks.BASIC_OFF,
            Util.ModBlocks.BASIC_LEVER,
            Util.ModBlocks.BASIC_LEVER_ON,
            Util.ModBlocks.BASIC_LEVER_OFF,
            Util.ModBlocks.BASIC_LEVER_SOLID,
            Util.ModBlocks.BASIC_LEVER_SOLID_ON,
            Util.ModBlocks.BASIC_LEVER_SOLID_OFF,
        };
        private static List<Color> filteredColors = unfilteredColors.Where(color => color != null).Select(color => color.Value).ToList();
        private readonly HashSet<Color> supportedBlockCodes = new HashSet<Color>(filteredColors);

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
                case var _ when Util.ModBlocks.BASIC_ON != null && blockCode == (Color)Util.ModBlocks.BASIC_ON:
                    return new BlockBasicOn(blockRect);
                case var _ when Util.ModBlocks.BASIC_OFF != null && blockCode == (Color)Util.ModBlocks.BASIC_OFF:
                    return new BlockBasicOff(blockRect);
                case var _ when Util.ModBlocks.BASIC_LEVER != null && blockCode == (Color)Util.ModBlocks.BASIC_LEVER:
                    return new BlockBasicLever(blockRect);
                case var _ when Util.ModBlocks.BASIC_LEVER_ON != null && blockCode == (Color)Util.ModBlocks.BASIC_LEVER_ON:
                    return new BlockBasicLeverOn(blockRect);
                case var _ when Util.ModBlocks.BASIC_LEVER_OFF != null && blockCode == (Color)Util.ModBlocks.BASIC_LEVER_OFF:
                    return new BlockBasicLeverOff(blockRect);
                case var _ when Util.ModBlocks.BASIC_LEVER_SOLID != null && blockCode == (Color)Util.ModBlocks.BASIC_LEVER_SOLID:
                    return new BlockBasicLeverSolid(blockRect);
                case var _ when Util.ModBlocks.BASIC_LEVER_SOLID_ON != null && blockCode == (Color)Util.ModBlocks.BASIC_LEVER_SOLID_ON:
                    return new BlockBasicLeverSolidOn(blockRect);
                case var _ when Util.ModBlocks.BASIC_LEVER_SOLID_OFF != null && blockCode == (Color)Util.ModBlocks.BASIC_LEVER_SOLID_OFF:
                    return new BlockBasicLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryBasic).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
