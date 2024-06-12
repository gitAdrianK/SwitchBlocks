using JumpKing.API;
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
    /// Factory for basic blocks.
    /// </summary>
    public class FactoryBasic : IBlockFactory
    {
        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            Util.ModBlocks.BASIC_ON,
            Util.ModBlocks.BASIC_OFF,
            Util.ModBlocks.BASIC_LEVER,
            Util.ModBlocks.BASIC_LEVER_ON,
            Util.ModBlocks.BASIC_LEVER_OFF,
            Util.ModBlocks.BASIC_LEVER_SOLID,
            Util.ModBlocks.BASIC_LEVER_SOLID_ON,
            Util.ModBlocks.BASIC_LEVER_SOLID_OFF,
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
                case var _ when blockCode == Util.ModBlocks.BASIC_ON:
                    return new BlockBasicOn(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_OFF:
                    return new BlockBasicOff(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_LEVER:
                    return new BlockBasicLever(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_LEVER_ON:
                    return new BlockBasicLeverOn(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_LEVER_OFF:
                    return new BlockBasicLeverOff(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_LEVER_SOLID:
                    return new BlockBasicLeverSolid(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_LEVER_SOLID_ON:
                    return new BlockBasicLeverSolidOn(blockRect);
                case var _ when blockCode == Util.ModBlocks.BASIC_LEVER_SOLID_OFF:
                    return new BlockBasicLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryBasic).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
