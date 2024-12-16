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
    public class FactoryJump : IBlockFactory
    {
        public static ulong LastUsedMapId { get; set; } = ulong.MaxValue;

        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            ModBlocks.JUMP_ON,
            ModBlocks.JUMP_OFF,
            ModBlocks.JUMP_ICE_ON,
            ModBlocks.JUMP_ICE_OFF,
            ModBlocks.JUMP_SNOW_ON,
            ModBlocks.JUMP_SNOW_OFF,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.JUMP_ON:
                case var _ when blockCode == ModBlocks.JUMP_OFF:
                case var _ when blockCode == ModBlocks.JUMP_ICE_ON:
                case var _ when blockCode == ModBlocks.JUMP_ICE_OFF:
                case var _ when blockCode == ModBlocks.JUMP_SNOW_ON:
                case var _ when blockCode == ModBlocks.JUMP_SNOW_OFF:
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
                case var _ when blockCode == ModBlocks.JUMP_ON:
                    return new BlockJumpOn(blockRect);
                case var _ when blockCode == ModBlocks.JUMP_OFF:
                    return new BlockJumpOff(blockRect);
                case var _ when blockCode == ModBlocks.JUMP_ICE_ON:
                    return new BlockJumpIceOn(blockRect);
                case var _ when blockCode == ModBlocks.JUMP_ICE_OFF:
                    return new BlockJumpIceOff(blockRect);
                case var _ when blockCode == ModBlocks.JUMP_SNOW_ON:
                    return new BlockJumpSnowOn(blockRect);
                case var _ when blockCode == ModBlocks.JUMP_SNOW_OFF:
                    return new BlockJumpSnowOff(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryJump).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
