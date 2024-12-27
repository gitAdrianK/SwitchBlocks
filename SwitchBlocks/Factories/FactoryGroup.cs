namespace SwitchBlocks.Factories
{
    using System;
    using System.Collections.Generic;
    using JumpKing.API;
    using JumpKing.Level;
    using JumpKing.Level.Sampler;
    using JumpKing.Workshop;
    using Microsoft.Xna.Framework;
    using SwitchBlocks.Blocks;
    using SwitchBlocks.Setups;

    /// <summary>
    /// Factory for group blocks.
    /// </summary>
    public class FactoryGroup : IBlockFactory
    {
        public static ulong LastUsedMapId { get; private set; } = ulong.MaxValue;

        private static readonly HashSet<Color> SupportedBlockCodes = new HashSet<Color> {
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

        public bool CanMakeBlock(Color blockCode, Level level) => SupportedBlockCodes.Contains(blockCode);

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
                default:
                    break;
            }
            return false;
        }

        public IBlock GetBlock(Color blockCode, Rectangle blockRect, Level level, LevelTexture textureSrc, int currentScreen, int x, int y)
        {
            if (LastUsedMapId != level.ID && SupportedBlockCodes.Contains(blockCode))
            {
                SetupGroup.BlocksGroupA.Clear();
                SetupGroup.BlocksGroupB.Clear();
                SetupGroup.BlocksGroupC.Clear();
                SetupGroup.BlocksGroupD.Clear();
                LastUsedMapId = level.ID;
            }

            switch (blockCode)
            {
                // Position stored in a single integer.
                // X and Y can never be a three digit number.
                // Screen can never be a four digit number.
                // As such the integers form is 00...00SSSXXYY.
                case var _ when blockCode == ModBlocks.GROUP_A:
                    var blockGroupA = new BlockGroupA(blockRect);
                    SetupGroup.BlocksGroupA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupA;
                    return blockGroupA;
                case var _ when blockCode == ModBlocks.GROUP_B:
                    var blockGroupB = new BlockGroupB(blockRect);
                    SetupGroup.BlocksGroupB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupB;
                    return blockGroupB;
                case var _ when blockCode == ModBlocks.GROUP_C:
                    var blockGroupC = new BlockGroupC(blockRect);
                    SetupGroup.BlocksGroupC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupC;
                    return blockGroupC;
                case var _ when blockCode == ModBlocks.GROUP_D:
                    var blockGroupD = new BlockGroupD(blockRect);
                    SetupGroup.BlocksGroupD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupD;
                    return blockGroupD;
                case var _ when blockCode == ModBlocks.GROUP_ICE_A:
                    var blockGroupIceA = new BlockGroupIceA(blockRect);
                    SetupGroup.BlocksGroupA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceA;
                    return blockGroupIceA;
                case var _ when blockCode == ModBlocks.GROUP_ICE_B:
                    var blockGroupIceB = new BlockGroupIceB(blockRect);
                    SetupGroup.BlocksGroupB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceB;
                    return blockGroupIceB;
                case var _ when blockCode == ModBlocks.GROUP_ICE_C:
                    var blockGroupIceC = new BlockGroupIceC(blockRect);
                    SetupGroup.BlocksGroupC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceC;
                    return blockGroupIceC;
                case var _ when blockCode == ModBlocks.GROUP_ICE_D:
                    var blockGroupIceD = new BlockGroupIceD(blockRect);
                    SetupGroup.BlocksGroupD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupIceD;
                    return blockGroupIceD;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_A:
                    var blockGroupSnowA = new BlockGroupSnowA(blockRect);
                    SetupGroup.BlocksGroupA[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowA;
                    return blockGroupSnowA;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_B:
                    var blockGroupSnowB = new BlockGroupSnowB(blockRect);
                    SetupGroup.BlocksGroupB[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowB;
                    return blockGroupSnowB;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_C:
                    var blockGroupSnowC = new BlockGroupSnowC(blockRect);
                    SetupGroup.BlocksGroupC[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowC;
                    return blockGroupSnowC;
                case var _ when blockCode == ModBlocks.GROUP_SNOW_D:
                    var blockGroupSnowD = new BlockGroupSnowD(blockRect);
                    SetupGroup.BlocksGroupD[((currentScreen + 1) * 10000) + (x * 100) + y] = blockGroupSnowD;
                    return blockGroupSnowD;
                case var _ when blockCode == ModBlocks.GROUP_RESET:
                    return new BlockGroupReset(blockRect);
                case var _ when blockCode == ModBlocks.GROUP_RESET_SOLID:
                    return new BlockGroupResetSolid(blockRect);
                default:
                    throw new InvalidOperationException($"{nameof(FactoryGroup)} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
