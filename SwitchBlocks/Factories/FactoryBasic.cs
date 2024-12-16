using JumpKing.API;
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
    /// Factory for basic blocks.
    /// </summary>
    public class FactoryBasic : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            ModBlocks.BASIC_ON,
            ModBlocks.BASIC_OFF,
            ModBlocks.BASIC_ICE_ON,
            ModBlocks.BASIC_ICE_OFF,
            ModBlocks.BASIC_SNOW_ON,
            ModBlocks.BASIC_SNOW_OFF,
            ModBlocks.BASIC_LEVER,
            ModBlocks.BASIC_LEVER_ON,
            ModBlocks.BASIC_LEVER_OFF,
            ModBlocks.BASIC_LEVER_SOLID,
            ModBlocks.BASIC_LEVER_SOLID_ON,
            ModBlocks.BASIC_LEVER_SOLID_OFF,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.BASIC_ON:
                case var _ when blockCode == ModBlocks.BASIC_OFF:
                case var _ when blockCode == ModBlocks.BASIC_ICE_ON:
                case var _ when blockCode == ModBlocks.BASIC_ICE_OFF:
                case var _ when blockCode == ModBlocks.BASIC_SNOW_ON:
                case var _ when blockCode == ModBlocks.BASIC_SNOW_OFF:
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID:
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_ON:
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_OFF:
                    return true;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && supportedBlockCodes.Contains(blockCode))
            {
                LastUsedMapId = level.ID;
            }
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.BASIC_ON:
                    return new BlockBasicOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_OFF:
                    return new BlockBasicOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_ICE_ON:
                    return new BlockBasicIceOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_ICE_OFF:
                    return new BlockBasicIceOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_SNOW_ON:
                    return new BlockBasicSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_SNOW_OFF:
                    return new BlockBasicSnowOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER:
                    return new BlockBasicLever(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_ON:
                    return new BlockBasicLeverOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_OFF:
                    return new BlockBasicLeverOff(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID:
                    return new BlockBasicLeverSolid(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_ON:
                    return new BlockBasicLeverSolidOn(blockRect);
                case var _ when blockCode == ModBlocks.BASIC_LEVER_SOLID_OFF:
                    return new BlockBasicLeverSolidOff(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryBasic).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
