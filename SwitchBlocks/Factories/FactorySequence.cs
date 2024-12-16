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
    /// Factory for sequence blocks.
    /// </summary>
    public class FactorySequence : IBlockFactory
    {
        public static ulong LastUsedMapId { get; set; } = ulong.MaxValue;
        private ulong levelId = ulong.MaxValue;

        private static readonly HashSet<Color> supportedBlockCodes = new HashSet<Color> {
            ModBlocks.SEQUENCE_A,
            ModBlocks.SEQUENCE_B,
            ModBlocks.SEQUENCE_C,
            ModBlocks.SEQUENCE_D,
            ModBlocks.SEQUENCE_ICE_A,
            ModBlocks.SEQUENCE_ICE_B,
            ModBlocks.SEQUENCE_ICE_C,
            ModBlocks.SEQUENCE_ICE_D,
            ModBlocks.SEQUENCE_SNOW_A,
            ModBlocks.SEQUENCE_SNOW_B,
            ModBlocks.SEQUENCE_SNOW_C,
            ModBlocks.SEQUENCE_SNOW_D,
            ModBlocks.SEQUENCE_RESET,
            ModBlocks.SEQUENCE_RESET_SOLID,
        };

        public bool CanMakeBlock(Color blockCode, Level level)
        {
            return supportedBlockCodes.Contains(blockCode);
        }

        public bool IsSolidBlock(Color blockCode)
        {
            switch (blockCode)
            {
                case var _ when blockCode == ModBlocks.SEQUENCE_A:
                case var _ when blockCode == ModBlocks.SEQUENCE_B:
                case var _ when blockCode == ModBlocks.SEQUENCE_C:
                case var _ when blockCode == ModBlocks.SEQUENCE_D:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_A:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_B:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_C:
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_D:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_A:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_B:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_C:
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_D:
                case var _ when blockCode == ModBlocks.SEQUENCE_RESET_SOLID:
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
            if (level.ID != levelId)
            {
                SetupSequence.BlocksSequenceA.Clear();
                SetupSequence.BlocksSequenceB.Clear();
                SetupSequence.BlocksSequenceC.Clear();
                SetupSequence.BlocksSequenceD.Clear();
                levelId = level.ID;
            }

            switch (blockCode)
            {
                // Position stored in a single integer.
                // X and Y can never be a three digit number.
                // Screen can never be a four digit number.
                // As such the integers form is 00...00SSSXXYY.
                case var _ when blockCode == ModBlocks.SEQUENCE_A:
                    BlockSequenceA blockSequenceA = new BlockSequenceA(blockRect);
                    SetupSequence.BlocksSequenceA[currentScreen * 10000 + x * 100 + y] = blockSequenceA;
                    return blockSequenceA;
                case var _ when blockCode == ModBlocks.SEQUENCE_B:
                    BlockSequenceB blockSequenceB = new BlockSequenceB(blockRect);
                    SetupSequence.BlocksSequenceB[currentScreen * 10000 + x * 100 + y] = blockSequenceB;
                    return blockSequenceB;
                case var _ when blockCode == ModBlocks.SEQUENCE_C:
                    BlockSequenceC blockSequenceC = new BlockSequenceC(blockRect);
                    SetupSequence.BlocksSequenceC[currentScreen * 10000 + x * 100 + y] = blockSequenceC;
                    return blockSequenceC;
                case var _ when blockCode == ModBlocks.SEQUENCE_D:
                    BlockSequenceD blockSequenceD = new BlockSequenceD(blockRect);
                    SetupSequence.BlocksSequenceD[currentScreen * 10000 + x * 100 + y] = blockSequenceD;
                    return blockSequenceD;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_A:
                    BlockSequenceIceA blockSequenceIceA = new BlockSequenceIceA(blockRect);
                    SetupSequence.BlocksSequenceA[currentScreen * 10000 + x * 100 + y] = blockSequenceIceA;
                    return blockSequenceIceA;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_B:
                    BlockSequenceIceB blockSequenceIceB = new BlockSequenceIceB(blockRect);
                    SetupSequence.BlocksSequenceB[currentScreen * 10000 + x * 100 + y] = blockSequenceIceB;
                    return blockSequenceIceB;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_C:
                    BlockSequenceIceC blockSequenceIceC = new BlockSequenceIceC(blockRect);
                    SetupSequence.BlocksSequenceC[currentScreen * 10000 + x * 100 + y] = blockSequenceIceC;
                    return blockSequenceIceC;
                case var _ when blockCode == ModBlocks.SEQUENCE_ICE_D:
                    BlockSequenceIceD blockSequenceIceD = new BlockSequenceIceD(blockRect);
                    SetupSequence.BlocksSequenceD[currentScreen * 10000 + x * 100 + y] = blockSequenceIceD;
                    return blockSequenceIceD;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_A:
                    BlockSequenceSnowA blockSequenceSnowA = new BlockSequenceSnowA(blockRect);
                    SetupSequence.BlocksSequenceA[currentScreen * 10000 + x * 100 + y] = blockSequenceSnowA;
                    return blockSequenceSnowA;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_B:
                    BlockSequenceSnowB blockSequenceSnowB = new BlockSequenceSnowB(blockRect);
                    SetupSequence.BlocksSequenceB[currentScreen * 10000 + x * 100 + y] = blockSequenceSnowB;
                    return blockSequenceSnowB;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_C:
                    BlockSequenceSnowC blockSequenceSnowC = new BlockSequenceSnowC(blockRect);
                    SetupSequence.BlocksSequenceC[currentScreen * 10000 + x * 100 + y] = blockSequenceSnowC;
                    return blockSequenceSnowC;
                case var _ when blockCode == ModBlocks.SEQUENCE_SNOW_D:
                    BlockSequenceSnowD blockSequenceSnowD = new BlockSequenceSnowD(blockRect);
                    SetupSequence.BlocksSequenceD[currentScreen * 10000 + x * 100 + y] = blockSequenceSnowD;
                    return blockSequenceSnowD;
                case var _ when blockCode == ModBlocks.SEQUENCE_RESET:
                    return new BlockSequenceReset(blockRect);
                case var _ when blockCode == ModBlocks.SEQUENCE_RESET_SOLID:
                    return new BlockSequenceResetSolid(blockRect);
                default:
                    throw new InvalidOperationException($"{typeof(FactorySequence).Name} is unable to create a block of Color code ({blockCode.R}, {blockCode.G}, {blockCode.B})");
            }
        }
    }
}
