using JumpKing.API;
using JumpKing.Level;
using JumpKing.Level.Sampler;
using JumpKing.Workshop;
using Microsoft.Xna.Framework;
using SwitchBlocks.Blocks;
using SwitchBlocks.Setups;
using System;
using System.Collections.Generic;

namespace SwitchBlocks.Factories
{
    /// <summary>
    /// Factory for group blocks.
    /// </summary>
    public class FactoryGroup : IBlockFactory
    {
        private ulong levelId = ulong.MaxValue;

        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            ModBlocks.GROUP_A,
            ModBlocks.GROUP_B,
            ModBlocks.GROUP_C,
            ModBlocks.GROUP_D,
            ModBlocks.GROUP_ICE_A,
            ModBlocks.GROUP_ICE_B,
            ModBlocks.GROUP_ICE_C,
            ModBlocks.GROUP_ICE_D,
            ModBlocks.GROUP_SNOW_A,
            ModBlocks.GROUP_SNOW_B,
            ModBlocks.GROUP_SNOW_C,
            ModBlocks.GROUP_SNOW_D,
            ModBlocks.GROUP_RESET,
            ModBlocks.GROUP_RESET_SOLID,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.GROUP_A:
                case var _ when blockCode == ModBlocks.GROUP_B:
                case var _ when blockCode == ModBlocks.GROUP_C:
                case var _ when blockCode == ModBlocks.GROUP_D:
                case var _ when blockCode == ModBlocks.GROUP_ICE_A:
                case var _ when blockCode == ModBlocks.GROUP_ICE_B:
                case var _ when blockCode == ModBlocks.GROUP_ICE_C:
                case var _ when blockCode == ModBlocks.GROUP_ICE_D:
                case var _ when blockCode == ModBlocks.GROUP_SNOW_A:
                case var _ when blockCode == ModBlocks.GROUP_SNOW_B:
                case var _ when blockCode == ModBlocks.GROUP_SNOW_C:
                case var _ when blockCode == ModBlocks.GROUP_SNOW_D:
                case var _ when blockCode == ModBlocks.GROUP_RESET_SOLID:
                    return true;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (level.ID != levelId)
            {
                SetupGroup.BlocksGroupA.Clear();
                SetupGroup.BlocksGroupB.Clear();
                SetupGroup.BlocksGroupC.Clear();
                SetupGroup.BlocksGroupD.Clear();
                levelId = level.ID;
            }

            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.GROUP_A:
                    BlockGroupA blockGroupA = new BlockGroupA(blockRect);
                    SetupGroup.BlocksGroupA[currentScreen * 10000 + x * 100 + y] = blockGroupA;
                    return blockGroupA;
                case var _ when blockCode == ModBlocks.GROUP_B:
                    BlockGroupB blockGroupB = new BlockGroupB(blockRect);
                    SetupGroup.BlocksGroupB[currentScreen * 10000 + x * 100 + y] = blockGroupB;
                    return blockGroupB;
                case var _ when blockCode == ModBlocks.GROUP_C:
                    BlockGroupC blockGroupC = new BlockGroupC(blockRect);
                    SetupGroup.BlocksGroupC[currentScreen * 10000 + x * 100 + y] = blockGroupC;
                    return blockGroupC;
                case var _ when blockCode == ModBlocks.GROUP_D:
                    BlockGroupD blockGroupD = new BlockGroupD(blockRect);
                    SetupGroup.BlocksGroupD[currentScreen * 10000 + x * 100 + y] = blockGroupD;
                    return blockGroupD;
                case var _ when blockCode == ModBlocks.GROUP_ICE_A:
                    BlockGroupIceA blockGroupIceA = new BlockGroupIceA(blockRect);
                    SetupGroup.BlocksGroupA[currentScreen * 10000 + x * 100 + y] = blockGroupIceA;
                    return blockGroupIceA;
                case var _ when blockCode == ModBlocks.GROUP_ICE_B:
                    BlockGroupIceB blockGroupIceB = new BlockGroupIceB(blockRect);
                    SetupGroup.BlocksGroupB[currentScreen * 10000 + x * 100 + y] = blockGroupIceB;
                    return blockGroupIceB;
                case var _ when blockCode == ModBlocks.GROUP_ICE_C:
                    BlockGroupIceC blockGroupIceC = new BlockGroupIceC(blockRect);
                    SetupGroup.BlocksGroupC[currentScreen * 10000 + x * 100 + y] = blockGroupIceC;
                    return blockGroupIceC;
                case var _ when blockCode == ModBlocks.GROUP_ICE_D:
                    BlockGroupIceD blockGroupIceD = new BlockGroupIceD(blockRect);
                    SetupGroup.BlocksGroupD[currentScreen * 10000 + x * 100 + y] = blockGroupIceD;
                    return blockGroupIceD;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_A:
                    BlockGroupSnowA blockGroupSnowA = new BlockGroupSnowA(blockRect);
                    SetupGroup.BlocksGroupA[currentScreen * 10000 + x * 100 + y] = blockGroupSnowA;
                    return blockGroupSnowA;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_B:
                    BlockGroupSnowB blockGroupSnowB = new BlockGroupSnowB(blockRect);
                    SetupGroup.BlocksGroupB[currentScreen * 10000 + x * 100 + y] = blockGroupSnowB;
                    return blockGroupSnowB;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_C:
                    BlockGroupSnowC blockGroupSnowC = new BlockGroupSnowC(blockRect);
                    SetupGroup.BlocksGroupC[currentScreen * 10000 + x * 100 + y] = blockGroupSnowC;
                    return blockGroupSnowC;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_D:
                    BlockGroupSnowD blockGroupSnowD = new BlockGroupSnowD(blockRect);
                    SetupGroup.BlocksGroupD[currentScreen * 10000 + x * 100 + y] = blockGroupSnowD;
                    return blockGroupSnowD;
                case var _ when blockCode == ModBlocks.GROUP_RESET:
                    return new BlockGroupReset(blockRect);
                case var _ when blockCode == ModBlocks.GROUP_RESET_SOLID:
                    return new BlockGroupResetSolid(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactoryGroup).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
